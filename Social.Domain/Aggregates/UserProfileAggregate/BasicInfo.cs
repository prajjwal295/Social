using Social.Domain.Exceptions;
using Social.Domain.Validators.UserProfileValidators;

namespace Social.Domain.Aggregates.UserProfileAggegate
{
    public class BasicInfo
    {
        private BasicInfo() { }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }
        public string Phone { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string CurrentCity { get; private set; }
        public string ProfilePicutreUrl { get; private set; }
        public string ProfilePicturePublicId { get; set; }

        /// <summary>
        /// factory Method
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="emailAddress"></param>
        /// <param name="phone"></param>
        /// <param name="dob"></param>
        /// <param name="currentCity"></param>
        /// <returns></returns>
        public static BasicInfo CreateBasicInfo(string firstName, string lastName, string emailAddress, string phone, DateTime dob, string currentCity, string profilePictureUrl, string publicId)
        {
            //todo : validation
            var validator = new BasicInfoValidator();

            var objToValidate = new BasicInfo()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                Phone = phone,
                DateOfBirth = dob,
                CurrentCity = currentCity,
                ProfilePicutreUrl = profilePictureUrl,
                ProfilePicturePublicId = publicId
            };

            var validationResult = validator.Validate(objToValidate);

            if (validationResult.IsValid)
            {
                return objToValidate;
            }

            var exception = new UserProfileNotValidException("The user info is Not Valid");

            foreach (var error in validationResult.Errors) {
                exception.ValidationErrors.Add(error.ErrorMessage);
            }

            throw exception;
        }
    }
}
