using System.Net;
using Application.Services.Interface.Logger;
using Application.Services.Interface.Sms;
using Application.ViewModels.Sms;
using Common.Enums;
using Kavenegar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace Application.Services.Concrete.Sms;

public class SmsService : ISmsService
{
    private readonly IConfiguration _configuration;
    private readonly ICustomLoggerService<SmsService> _logger;

    public SmsService(IConfiguration configuration, ICustomLoggerService<SmsService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    ///////
    //// USE WHEN YOU HAVE PRO KAVENEGAR SERVICE 
    //////
    public async Task<ResponseSmsViewModel> SendSms(string phoneNumber, int code, SmsPatternEnum pattern)
    {
        var apiKey = _configuration.GetSection("Sms:ApiKey").Value;
        var url = _configuration.GetSection("Sms:ByPatternUrl").Value;
        var urlWithKey = string.Format(url, apiKey);

        var type = pattern.GetType();
        var name = Enum.GetName(type, pattern);

        var patternName = type.GetField(name)?.GetCustomAttributes(false).OfType<Pattern>().SingleOrDefault();


        var options = new RestClientOptions(urlWithKey)
        {
            MaxTimeout = -1
        };

        var client = new RestClient(options);

        var request = new RestRequest("lookup.json");

        request.AddParameter("receptor", phoneNumber);
        request.AddParameter("template", patternName.PatternName);
        request.AddParameter("token", code);
        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
            return new ResponseSmsViewModel
            {
                Success = true
            };

        return new ResponseSmsViewModel
        {
            Success = false,
            Exception = response.ErrorException
        };
    }

    public Task SendSmsBasicVersion(string phoneNumber, int code)
    {
        var apiKey = _configuration.GetSection("Sms:ApiKey").Value;
        var sender = _configuration.GetSection("Sms:SenderNum").Value;
        var message = $"کد ورود شما \n{code}";

        var kavenegarApi = new KavenegarApi(apiKey);
        var result = kavenegarApi.Send(sender, phoneNumber, message);

        _logger.LogInformation($"Verify code sent to {phoneNumber}");
        return Task.CompletedTask;
    }
}