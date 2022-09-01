using Microsoft.AspNetCore.Identity;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UseStringInterpolation
// ReSharper disable StringLiteralTypo

namespace IdentityService.Helpers;

public class PersianErrorDescriber: IdentityErrorDescriber
{
    public override IdentityError DuplicateEmail(string email)
    {
        return new IdentityError()
        {
            Code = nameof(DuplicateEmail),
            Description = string.Format("ایمیل {0} قبلا ثبت شده است", email)
        };
    }
    
    public override IdentityError DuplicateUserName(string userName)
    {
     
        return new IdentityError()
        {
            Code = nameof(DuplicateEmail),
            Description = string.Format("نام کاربری {0} قبلا ثبت شده است", userName)
        };
    }
    
    /// <summary>
        /// Returns the default <see cref="IdentityError"/>.
        /// </summary>
        /// <returns>The default <see cref="IdentityError"/>.</returns>
        public override IdentityError DefaultError()
        {
            return new IdentityError
            {
                Code = nameof(DefaultError),
                Description = "مشکلی پیش آمده"
            };
        }

    /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a password mismatch.
        /// </summary>
        /// <returns>An <see cref="IdentityError"/> indicating a password mismatch.</returns>
        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Code = nameof(PasswordMismatch),
                Description = "کلمه‌عبور اشتباه است"
            };
        }

        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating an invalid token.
        /// </summary>
        /// <returns>An <see cref="IdentityError"/> indicating an invalid token.</returns>
        public override IdentityError InvalidToken()
        {
            return new IdentityError
            {
                Code = nameof(InvalidToken),
                Description = "کد وارد شده اشتباه است"
            };
        }

        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating the specified user <paramref name="userName"/> is invalid.
        /// </summary>
        /// <param name="userName">The user name that is invalid.</param>
        /// <returns>An <see cref="IdentityError"/> indicating the specified user <paramref name="userName"/> is invalid.</returns>
        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = string.Format("نام کاربری {0} قابل قبول نیست. نام کاربری فقط می‌تواند شامل حروف و اعداد باشد", userName)
            };
        }

        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating the specified <paramref name="email"/> is invalid.
        /// </summary>
        /// <param name="email">The email that is invalid.</param>
        /// <returns>An <see cref="IdentityError"/> indicating the specified <paramref name="email"/> is invalid.</returns>
        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = string.Format("ایمیل {0} قابل قبول نیست", email)
            };
        }

        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a password of the specified <paramref name="length"/> does not meet the minimum length requirements.
        /// </summary>
        /// <param name="length">The length that is not long enough.</param>
        /// <returns>An <see cref="IdentityError"/> indicating a password of the specified <paramref name="length"/> does not meet the minimum length requirements.</returns>
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = string.Format("کلمه‌عبور باید حداقل {0} حرف باشد", length)
            };
        }

        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a password entered does not contain a non-alphanumeric character, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="IdentityError"/> indicating a password entered does not contain a non-alphanumeric character.</returns>
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = "کلمه‌عبور باید حداقل یک کاراکتر غیر از حرف و عدد داشته باشد"
            };
        }

        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a password entered does not contain a numeric character, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="IdentityError"/> indicating a password entered does not contain a numeric character.</returns>
        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = "کلمه‌عبور باید حداقل یک عدد داشته باشد"
            };
        }

        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a password entered does not contain a lower case letter, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="IdentityError"/> indicating a password entered does not contain a lower case letter.</returns>
        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = "کلمه‌عبور باید حداقل یک حرف کوچک داشته باشد"
            };
        }

        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a password entered does not contain an upper case letter, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="IdentityError"/> indicating a password entered does not contain an upper case letter.</returns>
        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "کلمه‌عبور باید حداقل یک حرف بزرگ داشته باشد"
            };
        }
}