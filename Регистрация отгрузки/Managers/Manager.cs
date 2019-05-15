using System.ComponentModel.DataAnnotations;


namespace Sales_registration
{
    public partial class Manager
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Patronymic { get; set; }

        [Required]
        [StringLength(50)]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public bool Roots { get; set; }

        public bool Blocking { get; set; }
    }
}
