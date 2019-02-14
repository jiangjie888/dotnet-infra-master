using System.ComponentModel.DataAnnotations;

namespace zyGIS.Models.TokenAuth
{
    public class ExternalAuthenticateModel
    {
        public const int MaxLoginProviderLength = 128;
  
        public const int MaxProviderKeyLength = 256;

        [Required]
        [StringLength(MaxLoginProviderLength)]
        public string AuthProvider { get; set; }

        [Required]
        [StringLength(MaxProviderKeyLength)]
        public string ProviderKey { get; set; }

        [Required]
        public string ProviderAccessCode { get; set; }
    }
}
