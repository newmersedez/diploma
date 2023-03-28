using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Diploma.ECC.Encryption.Key;
using Diploma.ECC.Math.Entities;
using Diploma.ECC.Math.Enums;
using Diploma.Storage;
using Google.Protobuf;
using Grpc.Net.Client;

// var word = "lalka";
//
// using var channel = GrpcChannel.ForAddress("https://localhost:5001");
//
// var client = new Signature.SignatureClient(channel);
//
var curve = new Curve(CurveName.secp256r1);
var keyGen = new KeysGenerator();
var sender = keyGen.CreateKeyPair(curve);

Console.WriteLine(sender.PrivateKey);
Console.WriteLine(sender.PublicKey.X);
Console.WriteLine(sender.PublicKey.Y);
// var request = new SignFileRequest { Content = ByteString.CopyFrom(Encoding.UTF8.GetBytes(word)), PrivateKey = sender.PrivateKey.ToString() };
//
// var response = await client.SignFileAsyncAsync(request);
//
// Console.WriteLine($"{response.R.ToStringUtf8()} : {response.S.ToStringUtf8()}");
//
// var verifyRequest = new VerifySignatureRequest
// {
//     X = ByteString.CopyFrom(sender.PublicKey.X.ToByteArray()),
//     Y = ByteString.CopyFrom(sender.PublicKey.Y.ToByteArray()),
//     R = response.R,
//     S = response.S,
//     Content = ByteString.CopyFrom(Encoding.UTF8.GetBytes(word))
// };
//
// var verify = await client.VerifySignatureAsyncAsync(verifyRequest);
//
// Console.WriteLine($"{verify.IsVerified}");
//

using var channel = GrpcChannel.ForAddress("https://localhost:5001");

var client = new Storage.StorageClient(channel);

var fileName = "1.png";
var fileContent = System.IO.File.ReadAllBytes(fileName);

var request = new UploadFileRequest
{
    Name = fileName,
    Content = ByteString.CopyFrom(fileContent)
};

var response = client.UploadFileAsync(request);

Console.WriteLine(response.Status);