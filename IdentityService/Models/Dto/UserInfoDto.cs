namespace IdentityService.Models.Dto;

public class UserInfoDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime RegisterDate { get; set; }
    public DateTime BirthDate { get; set; }
    public bool IsPublic { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}