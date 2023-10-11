using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazorCountJungLab.Context;
using MudBlazorCountJungLab.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MudBlazorCountJungLab.Provider
{
    public class GoogleAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
    {
        public GoogleUserCredential? CurrentUser { get; set; } = new();
        private bool disposedValue;
        public string AuthenticationToken { get; set; } = "";

        public GoogleAuthenticationStateProvider()
        {
            AuthenticationStateChanged += OnAuthenticationStateChangedAsync;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                    AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        // ~GoogleAuthenticationStateProvider()
        // {
        //     // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task)
        {
            var authenticationState = await task;

            if (authenticationState is not null)
            {
                CurrentUser = GoogleUserCredential.FromClaimsPrincipal(authenticationState.User);
            }
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var principal = new ClaimsPrincipal();
            var user = FetchUserFromBrowser();

            if (user is not null)
            {
                CurrentUser = user;
                principal = user.ToClaimsPrincipal();
            }

            return Task.FromResult<AuthenticationState>(new(principal));
        }

        public GoogleUserCredential? FetchUserFromBrowser()
        {
            var claimsPrincipal = CreateClaimsPrincipalFromToken(AuthenticationToken);
            var user = GoogleUserCredential.FromClaimsPrincipal(claimsPrincipal);

            return user;
        }

        private ClaimsPrincipal CreateClaimsPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var identity = new ClaimsIdentity();

            if (tokenHandler.CanReadToken(token))
            {
                var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
                identity = new ClaimsIdentity(jwtSecurityToken.Claims, "GoogleCredential");
            }

            return new ClaimsPrincipal(identity);
        }

        [JSInvokable]
        public void GoogleLogin(GoogleResponse googleResponse)
        {
            var principal = new ClaimsPrincipal();
            var user = GoogleUserCredential.GetUserInfoFromGoogleJWT(googleResponse.Credential);
            CurrentUser = user;

            if (user is not null)
            {
                principal = user.ToClaimsPrincipal();
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public void Logout()
        {
            AuthenticationToken = "";
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
        }
    }
}
