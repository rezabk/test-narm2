namespace Application.DTO;

public class CurrentUserInformationDTO
{
    public int UserInfoId { get; set; }
    public int UserIdentityId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string NationalCode { get; set; }
}