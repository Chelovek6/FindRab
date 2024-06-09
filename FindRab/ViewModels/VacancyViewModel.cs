using System.ComponentModel.DataAnnotations;

namespace FindRab.ViewModels
{
    public class VacancyViewModel
    {
        [Required]
        public int VacancyId { get; set; }  // Пользователь не может менять это поле

        [Required(ErrorMessage = "Название вакансии обязательно")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Название вакансии должно содержать от 3 до 100 символов")]
        [RegularExpression(@"^[А-Яа-яA-Za-z\s]+$", ErrorMessage = "Название вакансии может содержать только русские и английские буквы и пробелы")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Описание вакансии обязательно")]
        [MinLength(20, ErrorMessage = "Описание должно содержать минимум 20 символов")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Уровень образования обязателен")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Уровень образования должен содержать минимум 10 символов")]
        [RegularExpression(@"^[А-Яа-яA-Za-z\s]+$", ErrorMessage = "Уровень образования может содержать только русские и английские буквы и пробелы")]
        public string Education { get; set; }

        [Required(ErrorMessage = "Зарплата обязательна")]
        [Range(0, double.MaxValue, ErrorMessage = "Зарплата должна быть положительным числом")]
        public decimal Salary { get; set; }

        [Required]
        public int UserId { get; set; }  // Пользователь не может менять это поле

        [Required(ErrorMessage = "Номер телефона обязателен")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Телефон может содержать только цифры")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат Email")]
        public string Email { get; set; }

        [Required]
        public int ApplicationCount { get; set; }  // Пользователь не может менять это поле

        public int CurrentUserId { get; set; }  // Добавлено, но пользователь не может менять это поле
    }
}


