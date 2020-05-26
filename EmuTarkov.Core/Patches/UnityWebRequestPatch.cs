using EmuTarkov.Common.Utils.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace EmuTarkov.Core.Patches
{
    class UnityWebRequestPatch : GenericPatch<UnityWebRequestPatch>
    {
        private static readonly CertificateHandler _certificateHandler = new FakeCertificateHandler();

        public UnityWebRequestPatch() : base(postfix: nameof(PatchPostfix))
        {

        }

        protected override MethodBase GetTargetMethod()
        {
            return typeof(UnityWebRequestTexture)
                .GetMethod(nameof(UnityWebRequestTexture.GetTexture), new[] { typeof(string) });
        }

        static void PatchPostfix(UnityWebRequest __result)
        {
            __result.certificateHandler = _certificateHandler;
            __result.disposeCertificateHandlerOnDispose = false;
            __result.timeout = 1000;
        }

        class FakeCertificateHandler : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData)
            {
                return true;
            }
        }
    }
}
