using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageSharingWithAuth.Filters
{
    public class ReferenceHeaderAttribute:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var _context = filterContext.HttpContext.Request.RequestContext;
            UrlHelper _urlHelper = new UrlHelper(_context);

            if(filterContext.HttpContext != null)
            {
                if (filterContext.HttpContext.Request.UrlReferrer == null)
                    throw new HttpException("Invalid data submitted");
                if (_urlHelper.IsLocalUrl(filterContext.HttpContext.Request.UrlReferrer.AbsoluteUri))
                    throw new HttpException("Form not from this website");
            }
        }
    }
}