using KawanApp.Interfaces;
using KawanApp.Models;
using KawanApp.ViewModels;
using KawanApp.ViewModels.Pages;
using Plugin.Media;
using Refit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KawanApp.Views.Pages
{
    public partial class ProfileImagePage : ContentPage
    {
        private string Pic;
        private bool IsOwnProfile = false;
        private IServerApi ServerApi => RestService.For<IServerApi>(App.Server);
        public ProfileImagePage(bool isownprofile, string pic)
        {
            InitializeComponent();
            Pic = pic;
            IsOwnProfile = isownprofile;
            this.BindingContext = new ProfileImagePageViewModel(isownprofile, pic);
        }

        protected override void OnAppearing()
        {
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#f3f3f3");
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.White;
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Send(this, "navigateBack"); //Send to App.xaml.cs
            return true;
        }

        private void BackIcon_Tapped(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "navigateBack"); //Send to App.xaml.cs
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            var accepted = await DisplayAlert("Note","Would you like to edit your profile picture?", "Yes", "No");
            if (accepted)
            {
                var option = await DisplayActionSheet(null, "Cancel", null, "Take Photo", "Gallery", "Remove");

                if (string.IsNullOrEmpty(option))
                    return;
                
                if (option.Equals("Take Photo")) //Take photo
                {
                    //Take photo
                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await DisplayAlert("Error", "No camera avaialble!", "Ok");
                        return;
                    }

                    string newfilename;
                    if (Pic == "n/a")
                        newfilename = "uploads/" + App.CurrentUser + ".jpg";
                    else
                        newfilename = Pic.Replace(".jpg", "1.jpg");

                    var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front,
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                        Directory = "User Images",
                        Name = newfilename
                    });

                    if (file == null)
                        return;

                    //Convert photo from stream to base 64
                    string base64image;
                    using (var memoryStream = new MemoryStream())
                    {
                        file.GetStream().CopyTo(memoryStream);
                        var byteImage = memoryStream.ToArray();

                        base64image = Convert.ToBase64String(byteImage);
                    }

                    //Upload image to server
                    if (App.NetworkStatus)
                    {
                        //Open camera
                        var rm = await ServerApi.UploadPhoto(new PhotoUpload() { CurrentUser = App.CurrentUser, FileName = Pic, Base64 = base64image, UserType = App.CurrentUserType });
                        if (rm.Status)
                        {
                            profileImage.Source = ImageSource.FromStream(() =>
                            {
                                var stream = file.GetStream();
                                return stream;
                            });
                            MessagingCenter.Send(this, "updatePic", newfilename); //Send to view a profile page and profile image page view model
                            await DisplayAlert("Success", "Image uploaded.", "Ok");
                        }
                        else
                            await DisplayAlert("Failure", "A problem happened while uploading your image.", "Ok");
                    }
                    else
                        await DisplayAlert("Error", "Please turn on internet", "Ok");

                    file.Dispose();
                }
                else if(option.Equals("Gallery")) //Select from gallery
                {
                    //Open gallery
                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        await DisplayAlert("Error", "Permission not granted to photos!", "Ok");
                        return;
                    }

                    string newfilename;
                    if (Pic == "n/a")
                        newfilename = "uploads/" + App.CurrentUser + ".jpg";
                    else
                        newfilename = Pic.Replace(".jpg", "1.jpg");

                    var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                    {
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                    });


                    if (file == null)
                        return;

                    //Convert photo from stream to base 64
                    string base64image;
                    using (var memoryStream = new MemoryStream())
                    {
                        file.GetStream().CopyTo(memoryStream);
                        var byteImage = memoryStream.ToArray();

                        base64image = Convert.ToBase64String(byteImage);
                    }

                    //Upload image to server
                    if (App.NetworkStatus)
                    {
                        //Open camera
                        var rm = await ServerApi.UploadPhoto(new PhotoUpload() { CurrentUser = App.CurrentUser, FileName = Pic, Base64 = base64image, UserType = App.CurrentUserType });
                        if (rm.Status)
                        {
                            profileImage.Source = ImageSource.FromStream(() =>
                            {
                                var stream = file.GetStream();
                                return stream;
                            });
                            await Task.Run(() => { MessagingCenter.Send(this, "updatePic", newfilename); }); //Send to view a profile page and profile image page view model
                            await DisplayAlert("Success", "Image uploaded.", "Ok");
                        }
                        else
                            await DisplayAlert("Failure", "A problem happened while uploading your image.", "Ok");
                    }
                    else
                        await DisplayAlert("Error", "Please turn on internet", "Ok");

                    file.Dispose();
                }
                else if (option.Equals("Remove")) //Remove photo
                {
                    await DisplayAlert("Success", "Your photo has been removed.", "Ok");
                    //Remove photo
                }

            }
            else
                return;
        }
    }
}