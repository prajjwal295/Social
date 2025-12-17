namespace Social.Api.Extenstions
{
    public static class HttpContextExtension
    {
        public static Guid GetUserProfileIdClaimValue(this HttpContext context)
        {
            return GetClaimValue("UserProfileId", context);
        }

        public static Guid GetIdentityIdClaimValue(this HttpContext context) { 
            return GetClaimValue("IdentityId" , context);
        }

        private static Guid GetClaimValue(string key, HttpContext httpContext)
        {
            var claims = httpContext.User.Claims;
            var value = claims.FirstOrDefault(x => x.Type == key)?.Value;

            return Guid.Parse(value);
        }
    }
}
