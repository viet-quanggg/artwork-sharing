using ArtworkSharing.Core.Interfaces.Services;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;

namespace ArtworkSharing.Service.Services;

public class FireBaseService : IFireBaseService
{
    private static readonly string ApiKey = "AIzaSyB-4-AYXKKdtIMZ04WWO38cLec56loIAt0";
    private static readonly string Bucket = "recipeorganizer-58fca.appspot.com";
    private static readonly string AuthEmail = "recipeorganizert3@gmail.com";
    private static readonly string AuthPassword = "recipeorganizer123";

    public async Task<string> UploadImageSingleNotList(IFormFile files)
    {
        var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

        // get authentication token
        var authResultTask = auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
        var authResult = await authResultTask;
        var token = authResult.FirebaseToken;


        var imageLink = "";
        var firebaseImageModel = new FirebaseImageModel();

        if (files.Length > 0)
        {
            firebaseImageModel.ImageFile = files;
            var stream = firebaseImageModel.ImageFile.OpenReadStream();
            //you can use CancellationTokenSource to cancel the upload midway
            var cancellation = new CancellationTokenSource();

            var result = await new FirebaseStorage(Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(token)
                    })
                .Child("products")
                .Child(firebaseImageModel.ImageFile.FileName)
                .PutAsync(stream, cancellation.Token);

            cancellation.Cancel();
            imageLink += result;
        }

        return imageLink;
    }

    public async Task<string> UploadImageSingle(List<IFormFile> files)
    {
        var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

        // get authentication token
        var authResultTask = auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
        var authResult = await authResultTask;
        var token = authResult.FirebaseToken;


        var imageLink = "";
        var firebaseImageModel = new FirebaseImageModel();
        foreach (var file in files)
            if (file.Length > 0)
            {
                firebaseImageModel.ImageFile = file;
                var stream = firebaseImageModel.ImageFile.OpenReadStream();
                //you can use CancellationTokenSource to cancel the upload midway
                var cancellation = new CancellationTokenSource();

                var result = await new FirebaseStorage(Bucket,
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(token)
                        })
                    .Child("products")
                    .Child(firebaseImageModel.ImageFile.FileName)
                    .PutAsync(stream, cancellation.Token);

                cancellation.Cancel();
                imageLink += result;
            }

        return imageLink;
    }

    public async Task<string> UploadImage(List<IFormFile> files)
    {
        var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

        // get authentication token
        var authResultTask = auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
        var authResult = await authResultTask;
        var token = authResult.FirebaseToken;


        var imageLink = "";
        var firebaseImageModel = new FirebaseImageModel();
        foreach (var file in files)
            if (file.Length > 0)
            {
                firebaseImageModel.ImageFile = file;
                var stream = firebaseImageModel.ImageFile.OpenReadStream();
                //you can use CancellationTokenSource to cancel the upload midway
                var cancellation = new CancellationTokenSource();

                var result = await new FirebaseStorage(Bucket,
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(token)
                        })
                    .Child("products")
                    .Child(firebaseImageModel.ImageFile.FileName)
                    .PutAsync(stream, cancellation.Token);

                cancellation.Cancel();
                imageLink += result + "ygbygyn34897gnygytfrfr";
            }

        return imageLink;
    }

    public async Task<string> UploadImages(List<IFormFile> files)
    {
        var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

        // get authentication token
        var authResultTask = auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
        var authResult = await authResultTask;
        var token = authResult.FirebaseToken;

        var imageLink = "";
        var firebaseImageModel = new FirebaseImageModel();

        foreach (var file in files)
            if (file.Length > 0)
            {
                // Check if the file is an image
                if (IsImage(file))
                {
                    firebaseImageModel.ImageFile = file;
                    var stream = firebaseImageModel.ImageFile.OpenReadStream();
                    //you can use CancellationTokenSource to cancel the upload midway
                    var cancellation = new CancellationTokenSource();

                    var result = await new FirebaseStorage(Bucket,
                            new FirebaseStorageOptions
                            {
                                AuthTokenAsyncFactory = () => Task.FromResult(token)
                            })
                        .Child("products")
                        .Child(firebaseImageModel.ImageFile.FileName)
                        .PutAsync(stream, cancellation.Token);

                    cancellation.Cancel();
                    imageLink += result + "ygbygyn34897gnygytfrfr";
                }
                else
                {
                    // Invalid image format
                    throw new Exception("Invalid image format. Only JPG, PNG, and GIF formats are allowed.");
                }
            }

        return imageLink;
    }

    private bool IsImage(IFormFile file)
    {
        // Check the file's content type to determine if it is an image
        var allowedFormats = new[] { "image/jpeg", "image/png", "image/gif" };
        return allowedFormats.Contains(file.ContentType);
    }
}

public class FirebaseImageModel
{
    public IFormFile ImageFile { get; set; }
}