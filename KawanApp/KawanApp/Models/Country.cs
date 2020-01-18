using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class Country
    {
        public string CountryName { get; set; }
        public string CountryFlag { get {
                string cf = "";
                switch (CountryName)
                {
                    case "Ascension Island":
                        cf = "🇦🇨";
                        break;

                    case "Andorra":
                        cf = "🇦🇩";
                        break;

                    case "United Arab Emirates":
                        cf = "🇦🇪";
                        break;

                    case "Afghanistan":
                        cf = "🇦🇫";
                        break;

                    case "Antigua & Barbuda":
                        cf = "🇦🇬";
                        break;

                    case "Anguilla":
                        cf = "🇦🇮";
                        break;

                    case "Albania":
                        cf = "🇦🇱";
                        break;

                    case "Armenia":
                        cf = "🇦🇲";
                        break;

                    case "Angola":
                        cf = "🇦🇴";
                        break;

                    case "Antarctica":
                        cf = "🇦🇶";
                        break;

                    case "Argentina":
                        cf = "🇦🇷";
                        break;

                    case "American Samoa":
                        cf = "🇦🇸";
                        break;

                    case "Austria":
                        cf = "🇦🇹";
                        break;

                    case "Australia":
                        cf = "🇦🇺";
                        break;

                    case "Aruba":
                        cf = "🇦🇼";
                        break;

                    case "Åland Islands":
                        cf = "🇦🇽";
                        break;

                    case "Azerbaijan":
                        cf = "🇦🇿";
                        break;

                    case "Bosnia & Herzegovina":
                        cf = "🇧🇦";
                        break;

                    case "Barbados":
                        cf = "🇧🇧";
                        break;

                    case "Bangladesh":
                        cf = "🇧🇩";
                        break;

                    case "Belgium":
                        cf = "🇧🇪";
                        break;

                    case "Burkina Faso":
                        cf = "🇧🇫";
                        break;

                    case "Bulgaria":
                        cf = "🇧🇬";
                        break;

                    case "Bahrain":
                        cf = "🇧🇭";
                        break;

                    case "Burundi":
                        cf = "🇧🇮";
                        break;

                    case "Benin":
                        cf = "🇧🇯";
                        break;

                    case "St. Barthélemy":
                        cf = "🇧🇱";
                        break;

                    case "Bermuda":
                        cf = "🇧🇲";
                        break;

                    case "Brunei":
                        cf = "🇧🇳";
                        break;

                    case "Bolivia":
                        cf = "🇧🇴";
                        break;

                    case "Caribbean Netherlands":
                        cf = "🇧🇶";
                        break;

                    case "Brazil":
                        cf = "🇧🇷";
                        break;

                    case "Bahamas":
                        cf = "🇧🇸";
                        break;

                    case "Bhutan":
                        cf = "🇧🇹";
                        break;

                    case "Bouvet Island":
                        cf = "🇧🇻";
                        break;

                    case "Botswana":
                        cf = "🇧🇼";
                        break;

                    case "Belarus":
                        cf = "🇧🇾";
                        break;

                    case "Belize":
                        cf = "🇧🇿";
                        break;

                    case "Canada":
                        cf = "🇨🇦";
                        break;

                    case "Cocos (Keeling) Islands":
                        cf = "🇨🇨";
                        break;

                    case "Congo - Kinshasa":
                        cf = "🇨🇩";
                        break;

                    case "Central African Republic":
                        cf = "🇨🇫";
                        break;

                    case "Congo - Brazzaville":
                        cf = "🇨🇬";
                        break;

                    case "Switzerland":
                        cf = "🇨🇭";
                        break;

                    case "Côte d’Ivoire":
                        cf = "🇨🇮";
                        break;

                    case "Cook Islands":
                        cf = "🇨🇰";
                        break;

                    case "Chile":
                        cf = "🇨🇱";
                        break;

                    case "Cameroon":
                        cf = "🇨🇲";
                        break;

                    case "China":
                        cf = "🇨🇳";
                        break;

                    case "Colombia":
                        cf = "🇨🇴";
                        break;

                    case "Clipperton Island":
                        cf = "🇨🇵";
                        break;

                    case "Costa Rica":
                        cf = "🇨🇷";
                        break;

                    case "Cuba":
                        cf = "🇨🇺";
                        break;

                    case "Cape Verde":
                        cf = "🇨🇻";
                        break;

                    case "Curaçao":
                        cf = "🇨🇼";
                        break;

                    case "Christmas Island":
                        cf = "🇨🇽";
                        break;

                    case "Cyprus":
                        cf = "🇨🇾";
                        break;

                    case "Czechia":
                        cf = "🇨🇿";
                        break;

                    case "Germany":
                        cf = "🇩🇪";
                        break;

                    case "Diego Garcia":
                        cf = "🇩🇬";
                        break;

                    case "Djibouti":
                        cf = "🇩🇯";
                        break;

                    case "Denmark":
                        cf = "🇩🇰";
                        break;

                    case "Dominica":
                        cf = "🇩🇲";
                        break;

                    case "Dominican Republic":
                        cf = "🇩🇴";
                        break;

                    case "Algeria":
                        cf = "🇩🇿";
                        break;

                    case "Ceuta & Melilla":
                        cf = "🇪🇦";
                        break;

                    case "Ecuador":
                        cf = "🇪🇨";
                        break;

                    case "Estonia":
                        cf = "🇪🇪";
                        break;

                    case "Egypt":
                        cf = "🇪🇬";
                        break;

                    case "Western Sahara":
                        cf = "🇪🇭";
                        break;

                    case "Eritrea":
                        cf = "🇪🇷";
                        break;

                    case "Spain":
                        cf = "🇪🇸";
                        break;

                    case "Ethiopia":
                        cf = "🇪🇹";
                        break;

                    case "European Union":
                        cf = "🇪🇺";
                        break;

                    case "Finland":
                        cf = "🇫🇮";
                        break;

                    case "Fiji":
                        cf = "🇫🇯";
                        break;

                    case "Falkland Islands":
                        cf = "🇫🇰";
                        break;

                    case "Micronesia":
                        cf = "🇫🇲";
                        break;

                    case "Faroe Islands":
                        cf = "🇫🇴";
                        break;

                    case "France":
                        cf = "🇫🇷";
                        break;

                    case "Gabon":
                        cf = "🇬🇦";
                        break;

                    case "United Kingdom":
                        cf = "🇬🇧";
                        break;

                    case "Grenada":
                        cf = "🇬🇩";
                        break;

                    case "Georgia":
                        cf = "🇬🇪";
                        break;

                    case "French Guiana":
                        cf = "🇬🇫";
                        break;

                    case "Guernsey":
                        cf = "🇬🇬";
                        break;

                    case "Ghana":
                        cf = "🇬🇭";
                        break;

                    case "Gibraltar":
                        cf = "🇬🇮";
                        break;

                    case "Greenland":
                        cf = "🇬🇱";
                        break;

                    case "Gambia":
                        cf = "🇬🇲";
                        break;

                    case "Guinea":
                        cf = "🇬🇳";
                        break;

                    case "Guadeloupe":
                        cf = "🇬🇵";
                        break;

                    case "Equatorial Guinea":
                        cf = "🇬🇶";
                        break;

                    case "Greece":
                        cf = "🇬🇷";
                        break;

                    case "South Georgia & South Sandwich Islands":
                        cf = "🇬🇸";
                        break;

                    case "Guatemala":
                        cf = "🇬🇹";
                        break;

                    case "Guam":
                        cf = "🇬🇺";
                        break;

                    case "Guinea-Bissau":
                        cf = "🇬🇼";
                        break;

                    case "Guyana":
                        cf = "🇬🇾";
                        break;

                    case "Hong Kong SAR China":
                        cf = "🇭🇰";
                        break;

                    case "Heard & McDonald Islands":
                        cf = "🇭🇲";
                        break;

                    case "Honduras":
                        cf = "🇭🇳";
                        break;

                    case "Croatia":
                        cf = "🇭🇷";
                        break;

                    case "Haiti":
                        cf = "🇭🇹";
                        break;

                    case "Hungary":
                        cf = "🇭🇺";
                        break;

                    case "Canary Islands":
                        cf = "🇮🇨";
                        break;

                    case "Indonesia":
                        cf = "🇮🇩";
                        break;

                    case "Ireland":
                        cf = "🇮🇪";
                        break;

                    case "Israel":
                        cf = "🇮🇱";
                        break;

                    case "Isle of Man":
                        cf = "🇮🇲";
                        break;

                    case "India":
                        cf = "🇮🇳";
                        break;

                    case "British Indian Ocean Territory":
                        cf = "🇮🇴";
                        break;

                    case "Iraq":
                        cf = "🇮🇶";
                        break;

                    case "Iran":
                        cf = "🇮🇷";
                        break;

                    case "Iceland":
                        cf = "🇮🇸";
                        break;

                    case "Italy":
                        cf = "🇮🇹";
                        break;

                    case "Jersey":
                        cf = "🇯🇪";
                        break;

                    case "Jamaica":
                        cf = "🇯🇲";
                        break;

                    case "Jordan":
                        cf = "🇯🇴";
                        break;

                    case "Japan":
                        cf = "🇯🇵";
                        break;

                    case "Kenya":
                        cf = "🇰🇪";
                        break;

                    case "Kyrgyzstan":
                        cf = "🇰🇬";
                        break;

                    case "Cambodia":
                        cf = "🇰🇭";
                        break;

                    case "Kiribati":
                        cf = "🇰🇮";
                        break;

                    case "Comoros":
                        cf = "🇰🇲";
                        break;

                    case "St. Kitts & Nevis":
                        cf = "🇰🇳";
                        break;

                    case "North Korea":
                        cf = "🇰🇵";
                        break;

                    case "South Korea":
                        cf = "🇰🇷";
                        break;

                    case "Kuwait":
                        cf = "🇰🇼";
                        break;

                    case "Cayman Islands":
                        cf = "🇰🇾";
                        break;

                    case "Kazakhstan":
                        cf = "🇰🇿";
                        break;

                    case "Laos":
                        cf = "🇱🇦";
                        break;

                    case "Lebanon":
                        cf = "🇱🇧";
                        break;

                    case "St. Lucia":
                        cf = "🇱🇨";
                        break;

                    case "Liechtenstein":
                        cf = "🇱🇮";
                        break;

                    case "Sri Lanka":
                        cf = "🇱🇰";
                        break;

                    case "Liberia":
                        cf = "🇱🇷";
                        break;

                    case "Lesotho":
                        cf = "🇱🇸";
                        break;

                    case "Lithuania":
                        cf = "🇱🇹";
                        break;

                    case "Luxembourg":
                        cf = "🇱🇺";
                        break;

                    case "Latvia":
                        cf = "🇱🇻";
                        break;

                    case "Libya":
                        cf = "🇱🇾";
                        break;

                    case "Morocco":
                        cf = "🇲🇦";
                        break;

                    case "Monaco":
                        cf = "🇲🇨";
                        break;

                    case "Moldova":
                        cf = "🇲🇩";
                        break;

                    case "Montenegro":
                        cf = "🇲🇪";
                        break;

                    case "St. Martin":
                        cf = "🇲🇫";
                        break;

                    case "Madagascar":
                        cf = "🇲🇬";
                        break;

                    case "Marshall Islands":
                        cf = "🇲🇭";
                        break;

                    case "North Macedonia":
                        cf = "🇲🇰";
                        break;

                    case "Mali":
                        cf = "🇲🇱";
                        break;

                    case "Myanmar (Burma":
                        cf = "🇲🇲";
                        break;

                    case ") Mongolia ":
                        cf = ")🇲🇳";
                        break;

                    case "Macao Sar China":
                        cf = "🇲🇴";
                        break;

                    case "Northern Mariana Islands":
                        cf = "🇲🇵";
                        break;

                    case "Martinique":
                        cf = "🇲🇶";
                        break;

                    case "Mauritania":
                        cf = "🇲🇷";
                        break;

                    case "Montserrat":
                        cf = "🇲🇸";
                        break;

                    case "Malta":
                        cf = "🇲🇹";
                        break;

                    case "Mauritius":
                        cf = "🇲🇺";
                        break;

                    case "Maldives":
                        cf = "🇲🇻";
                        break;

                    case "Malawi":
                        cf = "🇲🇼";
                        break;

                    case "Mexico":
                        cf = "🇲🇽";
                        break;

                    case "Malaysia":
                        cf = "🇲🇾";
                        break;

                    case "Mozambique":
                        cf = "🇲🇿";
                        break;

                    case "Namibia":
                        cf = "🇳🇦";
                        break;

                    case "New Caledonia":
                        cf = "🇳🇨";
                        break;

                    case "Niger":
                        cf = "🇳🇪";
                        break;

                    case "Norfolk Island":
                        cf = "🇳🇫";
                        break;

                    case "Nigeria":
                        cf = "🇳🇬";
                        break;

                    case "Nicaragua":
                        cf = "🇳🇮";
                        break;

                    case "Netherlands":
                        cf = "🇳🇱";
                        break;

                    case "Norway":
                        cf = "🇳🇴";
                        break;

                    case "Nepal":
                        cf = "🇳🇵";
                        break;

                    case "Nauru":
                        cf = "🇳🇷";
                        break;

                    case "Niue":
                        cf = "🇳🇺";
                        break;

                    case "New Zealand":
                        cf = "🇳🇿";
                        break;

                    case "Oman":
                        cf = "🇴🇲";
                        break;

                    case "Panama":
                        cf = "🇵🇦";
                        break;

                    case "Peru":
                        cf = "🇵🇪";
                        break;

                    case "French Polynesia":
                        cf = "🇵🇫";
                        break;

                    case "Papua New Guinea":
                        cf = "🇵🇬";
                        break;

                    case "Philippines":
                        cf = "🇵🇭";
                        break;

                    case "Pakistan":
                        cf = "🇵🇰";
                        break;

                    case "Poland":
                        cf = "🇵🇱";
                        break;

                    case "St. Pierre & Miquelon":
                        cf = "🇵🇲";
                        break;

                    case "Pitcairn Islands":
                        cf = "🇵🇳";
                        break;

                    case "Puerto Rico":
                        cf = "🇵🇷";
                        break;

                    case "Palestinian Territories":
                        cf = "🇵🇸";
                        break;

                    case "Portugal":
                        cf = "🇵🇹";
                        break;

                    case "Palau":
                        cf = "🇵🇼";
                        break;

                    case "Paraguay":
                        cf = "🇵🇾";
                        break;

                    case "Qatar":
                        cf = "🇶🇦";
                        break;

                    case "Réunion":
                        cf = "🇷🇪";
                        break;

                    case "Romania":
                        cf = "🇷🇴";
                        break;

                    case "Serbia":
                        cf = "🇷🇸";
                        break;

                    case "Russia":
                        cf = "🇷🇺";
                        break;

                    case "Rwanda":
                        cf = "🇷🇼";
                        break;

                    case "Saudi Arabia":
                        cf = "🇸🇦";
                        break;

                    case "Solomon Islands":
                        cf = "🇸🇧";
                        break;

                    case "Seychelles":
                        cf = "🇸🇨";
                        break;

                    case "Sudan":
                        cf = "🇸🇩";
                        break;

                    case "Sweden":
                        cf = "🇸🇪";
                        break;

                    case "Singapore":
                        cf = "🇸🇬";
                        break;

                    case "St. Helena":
                        cf = "🇸🇭";
                        break;

                    case "Slovenia":
                        cf = "🇸🇮";
                        break;

                    case "Svalbard & Jan Mayen":
                        cf = "🇸🇯";
                        break;

                    case "Slovakia":
                        cf = "🇸🇰";
                        break;

                    case "Sierra Leone":
                        cf = "🇸🇱";
                        break;

                    case "San Marino":
                        cf = "🇸🇲";
                        break;

                    case "Senegal":
                        cf = "🇸🇳";
                        break;

                    case "Somalia":
                        cf = "🇸🇴";
                        break;

                    case "Suriname":
                        cf = "🇸🇷";
                        break;

                    case "South Sudan":
                        cf = "🇸🇸";
                        break;

                    case "São Tomé & Príncipe":
                        cf = "🇸🇹";
                        break;

                    case "El Salvador":
                        cf = "🇸🇻";
                        break;

                    case "Sint Maarten":
                        cf = "🇸🇽";
                        break;

                    case "Syria":
                        cf = "🇸🇾";
                        break;

                    case "Eswatini":
                        cf = "🇸🇿";
                        break;

                    case "Tristan Da Cunha":
                        cf = "🇹🇦";
                        break;

                    case "Turks & Caicos Islands":
                        cf = "🇹🇨";
                        break;

                    case "Chad":
                        cf = "🇹🇩";
                        break;

                    case "French Southern Territories":
                        cf = "🇹🇫";
                        break;

                    case "Togo":
                        cf = "🇹🇬";
                        break;

                    case "Thailand":
                        cf = "🇹🇭";
                        break;

                    case "Tajikistan":
                        cf = "🇹🇯";
                        break;

                    case "Tokelau":
                        cf = "🇹🇰";
                        break;

                    case "Timor-Leste":
                        cf = "🇹🇱";
                        break;

                    case "Turkmenistan":
                        cf = "🇹🇲";
                        break;

                    case "Tunisia":
                        cf = "🇹🇳";
                        break;

                    case "Tonga":
                        cf = "🇹🇴";
                        break;

                    case "Turkey":
                        cf = "🇹🇷";
                        break;

                    case "Trinidad & Tobago":
                        cf = "🇹🇹";
                        break;

                    case "Tuvalu":
                        cf = "🇹🇻";
                        break;

                    case "Taiwan":
                        cf = "🇹🇼";
                        break;

                    case "Tanzania":
                        cf = "🇹🇿";
                        break;

                    case "Ukraine":
                        cf = "🇺🇦";
                        break;

                    case "Uganda":
                        cf = "🇺🇬";
                        break;

                    case "U.S. Outlying Islands":
                        cf = "🇺🇲";
                        break;

                    case "United Nations":
                        cf = "🇺🇳";
                        break;

                    case "United States":
                        cf = "🇺🇸";
                        break;

                    case "Uruguay":
                        cf = "🇺🇾";
                        break;

                    case "Uzbekistan":
                        cf = "🇺🇿";
                        break;

                    case "Vatican City":
                        cf = "🇻🇦";
                        break;

                    case "St. Vincent & Grenadines":
                        cf = "🇻🇨";
                        break;

                    case "Venezuela":
                        cf = "🇻🇪";
                        break;

                    case "British Virgin Islands":
                        cf = "🇻🇬";
                        break;

                    case "U.S. Virgin Islands":
                        cf = "🇻🇮";
                        break;

                    case "Vietnam":
                        cf = "🇻🇳";
                        break;

                    case "Vanuatu":
                        cf = "🇻🇺";
                        break;

                    case "Wallis & Futuna":
                        cf = "🇼🇫";
                        break;

                    case "Samoa":
                        cf = "🇼🇸";
                        break;

                    case "Kosovo":
                        cf = "🇽🇰";
                        break;

                    case "Yemen":
                        cf = "🇾🇪";
                        break;

                    case "Mayotte":
                        cf = "🇾🇹";
                        break;

                    case "South Africa":
                        cf = "🇿🇦";
                        break;

                    case "Zambia":
                        cf = "🇿🇲";
                        break;

                    case "Zimbabwe":
                        cf = "🇿🇼";
                        break;

                    case "England":
                        cf = "🏴�󠁢󠁥󠁮󠁧󠁿 ";
                        break;

                    case "Scotland":
                        cf = "🏴�󠁢󠁳󠁣󠁴󠁿 ";
                        break;

                    case "Wales":
                        cf = "🏴�󠁢󠁷󠁬󠁳󠁿 ";
                        break;
                }
                return cf; 
            } 
        }
    }
}
