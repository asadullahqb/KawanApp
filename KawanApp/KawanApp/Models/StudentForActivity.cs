namespace KawanApp.Models
{
    public class StudentForActivity
    {
        public int Index { get; set; }
        public User StudentInfo { get; set; }
        public string ShortenedFullName { 
            get 
            {
                if (StudentInfo == null)
                    return "";
                else if (string.IsNullOrEmpty(StudentInfo.FullName))
                    return "";
                else
                    return (StudentInfo.FullName.Length > 10) ? StudentInfo.FullName.Substring(0, 9) + "..." : StudentInfo.FullName; 
            } 
        } //Lazy to code out a converter instead (like I should)
        public bool IsChecked { get; set; }
    }
}
