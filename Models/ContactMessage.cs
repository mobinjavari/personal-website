using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Models;

public class ContactMessage
{
  [Key]
  public int Id { get; set; }

  [Required(ErrorMessage = "لطفا نام خود را وارد کنید")]
  [StringLength(100, ErrorMessage = "نام نمی‌تواند بیشتر از 100 کاراکتر باشد")]
  public string Name { get; set; } = string.Empty;

  [Required(ErrorMessage = "لطفا ایمیل خود را وارد کنید")]
  [EmailAddress(ErrorMessage = "لطفا یک ایمیل معتبر وارد کنید")]
  [StringLength(100)]
  public string Email { get; set; } = string.Empty;

  [Required(ErrorMessage = "لطفا موضوع خود را وارد کنید")]
  [StringLength(100, ErrorMessage = "موضوع نمی‌تواند بیشتر از 100 کاراکتر باشد")]
  public string Subject { get; set; } = string.Empty;

  [Required(ErrorMessage = "لطفا پیام خود را وارد کنید")]
  [StringLength(1000, ErrorMessage = "پیام نمی‌تواند بیشتر از 1000 کاراکتر باشد")]
  public string Message { get; set; } = string.Empty;

  public bool IsRead { get; set; } = false;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}