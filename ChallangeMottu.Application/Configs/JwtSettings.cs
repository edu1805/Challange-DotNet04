namespace ChallangeMottu.Application.Configs;

public class JwtSettings
{
    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public int ExpirationMinutes { get; set; } = 60;
    
    public string Secret { get; set; }
    
    public JwtValidationSettings Validation { get; set; } = new();
    
    public int ClockSkewMinutes { get; set; } = 5;
}

public class JwtValidationSettings
{
    public bool ValidateIssuerSigningKey { get; set; } = true;

    public bool ValidateIssuer { get; set; } = true;

    public bool ValidateAudience { get; set; } = true;

    public bool ValidateLifetime { get; set; } = true;

    public bool ValidateExpirationTime { get; set; } = true;

    public bool ValidateNotBefore { get; set; } = true;

    public bool RequireExpirationTime { get; set; } = true;

    public bool RequireSignedTokens { get; set; } = true;
}