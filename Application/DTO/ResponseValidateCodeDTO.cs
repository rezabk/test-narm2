using Common.Enums;

namespace Application.DTO;

public class ResponseValidateCodeDTO
{
    public MessageId? MessageId { get; set; }
    
    public bool Success { get; set; }
}