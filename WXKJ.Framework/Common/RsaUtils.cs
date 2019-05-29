using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;

namespace WXKJ.Framework.Common
{
    public static class RsaUtils
    {
        /// <summary>
        /// 获取一个RSA密钥(不存在则生成)
        /// </summary>
        /// <param name="keyDir">存放密钥的文件夹路径</param>
        /// <param name="withPrivate">获取私钥</param>
        /// <returns></returns>
        public static RsaSecurityKey GetRsaSecurityKey(string keyDir, bool withPrivate)
        {
            if (TryGetKeyParameters(keyDir, withPrivate, out var keyParams) == false)
            {
                keyParams = GenerateAndSaveKey(keyDir);
            }
            var key = new RsaSecurityKey(keyParams);
            return key;
        }
        /// <summary>
        /// 从本地文件中读取用来签发 Token 的 RSA Key
        /// </summary>
        /// <param name="filePath">存放密钥的文件夹路径</param>
        /// <param name="withPrivate"></param>
        /// <param name="keyParameters"></param>
        /// <returns></returns>
        public static bool TryGetKeyParameters(string filePath, bool withPrivate, out RSAParameters keyParameters)
        {
            var filename = withPrivate ? "key.json" : "key.public.json";
            filePath = Path.Combine(filePath, filename);
            keyParameters = default(RSAParameters);
            if (File.Exists(filePath) == false) return false;
            //keyParameters = JsonConvert.DeserializeObject<RsaParameterStorage>(File.ReadAllText(filePath)).MapTo<RSAParameters>();
            //keyParameters = JsonConvert.DeserializeObject<RSAParameters>(File.ReadAllText(filePath)); //不能直接序列化，需要先转成类对象
            var rsaParaStorage = JsonConvert.DeserializeObject<RsaParameterStorage>(File.ReadAllText(filePath));
            keyParameters.D = rsaParaStorage.D;
            keyParameters.DP = rsaParaStorage.DP;
            keyParameters.DQ = rsaParaStorage.DQ;
            keyParameters.Exponent = rsaParaStorage.Exponent;
            keyParameters.InverseQ = rsaParaStorage.InverseQ;
            keyParameters.Modulus = rsaParaStorage.Modulus;
            keyParameters.P = rsaParaStorage.P;
            keyParameters.Q = rsaParaStorage.Q;
            return true;
        }

        /// <summary>
        /// 生成并保存 RSA 公钥与私钥
        /// </summary>
        /// <param name="filePath">存放密钥的文件夹路径</param>
        /// <returns></returns>
        public static RSAParameters GenerateAndSaveKey(string filePath)
        {
            RSAParameters publicKeys, privateKeys;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    privateKeys = rsa.ExportParameters(true);
                    publicKeys = rsa.ExportParameters(false);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
            File.WriteAllText(Path.Combine(filePath, "key.json"), privateKeys.ToJsonString());
            File.WriteAllText(Path.Combine(filePath, "key.public.json"), publicKeys.ToJsonString());
            return privateKeys;
        }

        private static string ToJsonString(this RSAParameters parameters)
        {
            //var content = parameters.MapTo<RsaParameterStorage>();
            var content = new RsaParameterStorage()
            {
                D = parameters.D,
                DP = parameters.DP,
                DQ = parameters.DQ,
                Exponent = parameters.Exponent,
                InverseQ = parameters.InverseQ,
                Modulus = parameters.Modulus,
                P = parameters.P,
                Q = parameters.Q
            };

            return JsonConvert.SerializeObject(content);
        }

        private class RsaParameterStorage
        {
            public byte[] D { get; set; }
            public byte[] DP { get; set; }
            public byte[] DQ { get; set; }
            public byte[] Exponent { get; set; }
            public byte[] InverseQ { get; set; }
            public byte[] Modulus { get; set; }
            public byte[] P { get; set; }
            public byte[] Q { get; set; }
        }
    }
}
