using Application.ViewModels.Sms;
using Common;
using Common.Enums;

namespace Application.Services.Interface.Sms;

public interface ISmsService
{
    ///////
    //// USE WHEN YOU HAVE PRO KAVENEGAR SERVICE 
    //////
    Task<ResponseSmsViewModel> SendSms(string phoneNumber, int code, SmsPatternEnum pattern);
    
    ///////
    //// USE WHEN YOU HAVE BASIC KAVENEGAR SERVICE 
    //////
    Task SendSmsBasicVersion(string phoneNumber, int code);
}