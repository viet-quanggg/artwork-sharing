using ArtworkSharing.Core.Interfaces.Services;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using System.Reactive.Disposables;

namespace ArtworkSharing.Service.Services;

public class FireBaseService : IFireBaseService
{
    private static readonly string ApiKey = "AIzaSyB-4-AYXKKdtIMZ04WWO38cLec56loIAt0";
    private static readonly string Bucket = "recipeorganizer-58fca.appspot.com";
    private static readonly string AuthEmail = "recipeorganizert3@gmail.com";
    private static readonly string AuthPassword = "recipeorganizer123";

    public async Task<string> UploadImageSingle(IFormFile files)
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
    public async Task<string> UploadImageWatermarkIntoFireBase(byte[] imageBytes, string imageType)
    {
        var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

        // get authentication token
        var authResultTask = auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
        var authResult = await authResultTask;
        var token = authResult.FirebaseToken;

        var imageLink = "";

        // Kiểm tra xem mảng byte của hình ảnh không phải là rỗng và kiểm tra xem chuỗi MIME type được cung cấp
        // có hợp lệ không
        if (imageBytes != null && !string.IsNullOrEmpty(imageType))
        {
            // Tạo stream từ mảng byte của hình ảnh
            using (var stream = new MemoryStream(imageBytes))
            {
                // Bắt đầu quá trình tải lên hình ảnh lên Firebase Storage
                var result = await new FirebaseStorage(Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(token)
                    })
                    .Child("products")
                    .Child(Guid.NewGuid().ToString()) // Đổi tên ngẫu nhiên cho hình ảnh để tránh trùng lặp
                    .PutAsync(stream, new CancellationTokenSource().Token, $"image/{imageType}");

                // Lấy đường dẫn của hình ảnh sau khi tải lên thành công
                imageLink = result;
            }
        }

        return imageLink;
    }



    public async Task<string> Test(List<IFormFile> files)
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

    public async Task<string> Test2(List<IFormFile> files)
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

    public async Task<List<string>> UploadMultiImagesAsync(List<IFormFile> files)
    {
        var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
        List<string> images = new List<string>();
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
                    images.Add(result);
                }
                else
                {
                    // Invalid image format
                    throw new Exception("Invalid image format. Only JPG, PNG, and GIF formats are allowed.");
                }
            }

        return images;
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