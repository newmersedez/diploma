using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Diploma.Client;
using Diploma.ECC.Encryption.Key;
using Diploma.ECC.Math.Entities;
using Diploma.ECC.Math.Enums;
using Google.Protobuf;
using Grpc.Net.Client;

var word = "lalka";

using var channel = GrpcChannel.ForAddress("https://localhost:5001");

var client = new Signature.SignatureClient(channel);

var curve = new Curve(CurveName.secp256r1);
var keyGen = new KeysGenerator();
var sender = keyGen.GetKeyPair(curve);

var request = new SignFileRequest { Content = ByteString.CopyFrom(Encoding.UTF8.GetBytes(word)), PrivateKey = sender.PrivateKey.ToString() };

var response = await client.SignFileAsyncAsync(request);

Console.WriteLine($"{response.R.ToStringUtf8()} : {response.S.ToStringUtf8()}");

var verifyRequest = new VerifySignatureRequest
{
    X = ByteString.CopyFrom(sender.PublicKey.X.ToByteArray()),
    Y = ByteString.CopyFrom(sender.PublicKey.Y.ToByteArray()),
    R = response.R,
    S = response.S,
    Content = ByteString.CopyFrom(Encoding.UTF8.GetBytes(word))
};

var verify = await client.VerifySignatureAsyncAsync(verifyRequest);

Console.WriteLine($"{verify.IsVerified}");

// TODO: Проверить верификацию