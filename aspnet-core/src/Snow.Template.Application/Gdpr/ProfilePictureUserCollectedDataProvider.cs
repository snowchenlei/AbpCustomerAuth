using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Dependency;
using Snow.Template.Authorization.Users;
using Snow.Template.Dto;
using Snow.Template.Net.MimeTypes;
using Snow.Template.Storage;

namespace Snow.Template.Gdpr
{
    public class ProfilePictureUserCollectedDataProvider : IUserCollectedDataProvider, ITransientDependency
    {
        private readonly UserManager _userManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public ProfilePictureUserCollectedDataProvider(
            UserManager userManager,
            IBinaryObjectManager binaryObjectManager,
            ITempFileCacheManager tempFileCacheManager
        )
        {
            _userManager = userManager;
            _binaryObjectManager = binaryObjectManager;
            _tempFileCacheManager = tempFileCacheManager;
        }

        public async Task<List<FileDto>> GetFiles(UserIdentifier user)
        {
            var profilePictureId = (await _userManager.GetUserByIdAsync(user.UserId)).ProfilePictureId;
            if (!profilePictureId.HasValue)
            {
                return new List<FileDto>();
            }

            var profilePicture = await _binaryObjectManager.GetOrNullAsync(profilePictureId.Value);
            if (profilePicture == null)
            {
                return new List<FileDto>();
            }

            var file = new FileDto("ProfilePicture.png", MimeTypeNames.ImagePng);
            //_tempFileCacheManager.SetFile(file.FileToken, profilePicture.FilePath);

            return new List<FileDto> { file };
        }
    }
}