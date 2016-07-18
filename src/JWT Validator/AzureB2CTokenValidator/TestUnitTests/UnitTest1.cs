using System;
using System.IO;
using System.Threading.Tasks;
using JWTNetTestBed;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task ValidateWithRsaOnAzureFunction()
        {
            var url =
                "https://jwtparse.azurewebsites.net/api/JWTWithRsaValidator?code=r3gqblabmyfju0c6zbhknwyiimhdrdn1yld7";

            var issuer = "CentralAuthHost";
            var audience = "SomeDemoServer";

            var publicKey =
                "PFJTQUtleVZhbHVlPjxNb2R1bHVzPjNhSm1GcHhJR2EvcDhlRWVLNXg5SzZSMGVWclh2YjEyczRIRWplQ29XUStKYnFrb1VpM29jdnYxRjFLT2tOWUdVTHBUWitYajFxS3FOSXBlTWxLUTBwRFllcUpwYTRWYU14TGJtejRIbEFYNnhVZWRLdzNrM2EzL1ZkUGZSbXc3aUIrdjlXNlVCL2NJMm1Db05VZFhnM3hPVndyZnBDTFJzaU1pWkE4NENUSDVPZmNFZUVWanZzMm4rSjcvZ0JEelRVc1JWMFFqaXRVMWxKYkFBNzNGdVQ2aXhTZktTRUw3ZWhWRFdXZEFSWU1nNjFkc0R1THY5UWQrdWMvYmdjUEdpa3dWVXRwa3o0dUV1aXBwcTQ4bEpjQTFmU2VHekxER3pVbG1ESkY1MWt6V201ZndUUXovNU9MY1QrbVhLSmhzQXlMTUNNL21MS0E3UFordTFFWFVrdz09PC9Nb2R1bHVzPjxFeHBvbmVudD5BUUFCPC9FeHBvbmVudD48L1JTQUtleVZhbHVlPg==";
            var token = File.ReadAllText("testToken.txt");

            var result = await JWTValidateAgainstAzureFunction.Validate(url, token, publicKey, issuer, audience);

            Assert.IsTrue(result.IsValid);

        }

        [TestMethod]
        public void ValidateWithRsaKey()
        {
            var key = File.ReadAllText("testKeyRsa.txt");
            var token = File.ReadAllText("testToken.txt");

            var tokenFail = token.Replace("My1jMm", "-0---");

            var failResult = JwtValidator.ValidateWithRsaKey(tokenFail, key, "CentralAuthHost", "SomeDemoServer");
            Assert.IsFalse(failResult.IsValid);

            var failResult2 = JwtValidator.ValidateWithRsaKey(token, key, "CentralAuthHost2", "SomeDemoServer");
            Assert.IsFalse(failResult2.IsValid);

            var failResult3 = JwtValidator.ValidateWithRsaKey(token, key, "CentralAuthHost", "SomeDemoServer3");
            Assert.IsFalse(failResult3.IsValid);


            var result = JwtValidator.ValidateWithRsaKey(token, key, "CentralAuthHost", "SomeDemoServer");
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void ValidateWithJwkKey()
        {
            var key = File.ReadAllText("testKey.txt");
            var token = File.ReadAllText("testToken.txt");
            
            var result = JwtValidator.ValidateWithJwk(token, key, "https://login.microsoftonline.com/0a7110e8-b2aa-48cf-844f-c43e3533288d/v2.0/", "B2C_1_JordoTestSignInAndUp");
            Assert.IsTrue(result.IsValid);
        }


        [TestMethod]
        public async Task ValidateWithDiscovery()
        {
            var token = File.ReadAllText("testToken.txt");
            var result = await JwtDownloadKeyAndValidator.Validate(token, "B2C_1_JordoTestSignInAndUp");
            Assert.IsTrue(result.IsValid);

        }
    }
}
