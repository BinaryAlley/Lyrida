#region ========================================================================= USING =====================================================================================
using Castle.DynamicProxy;
#endregion

namespace Lyrida.Infrastructure.Common.Logging;

/// <summary>
/// Proxy class providing logging functionality for intercepted interfaces of BL
/// </summary>
/// <remarks>
/// Creation Date: 20th of June, 2021
/// </remarks>
public class ProxyInterceptor : IInterceptor
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly AsyncProxyInterceptor asyncProxyInterceptor;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="asyncProxyInterceptor">Injected class that provides asynchronous dynamic proxy interception support</param>
    public ProxyInterceptor(AsyncProxyInterceptor asyncProxyInterceptor)
    {
        this.asyncProxyInterceptor = asyncProxyInterceptor;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Intercepts a method and forwards it to an asynchronous dynamic proxy interceptor
    /// </summary>
    /// <param name="invocation">Encapsulates an invocation of a proxied method</param>
    public void Intercept(IInvocation invocation)
    {
        asyncProxyInterceptor.ToInterceptor().Intercept(invocation);
    }
    #endregion
}