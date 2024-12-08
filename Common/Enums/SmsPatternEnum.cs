namespace Common.Enums;

public enum SmsPatternEnum
{
    [Pattern("verify")] VerifyCode = 1,

 }

public class Pattern : Attribute
{
    public Pattern(string patternName)
    {
        _patternName = patternName;
    }

    private string _patternName { get; }

    public virtual string PatternName => _patternName;
}