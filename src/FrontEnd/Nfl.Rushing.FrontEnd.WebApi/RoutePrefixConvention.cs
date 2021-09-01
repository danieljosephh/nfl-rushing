using System;
using System.Collections.Generic;
using System.Linq;

using Castle.Core.Internal;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Nfl.Rushing.FrontEnd.WebApi
{
    public class RoutePrefixConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _routePrefix;

        public RoutePrefixConvention(IRouteTemplateProvider route)
        {
            this._routePrefix = new AttributeRouteModel(route);
        }

        public void Apply(ApplicationModel application)
        {
            var controllers = application.Controllers
                .Where(x => x.ControllerType.GetAttribute<ApiControllerAttribute>() != null)
                .ToList();
            foreach (var selector in controllers.SelectMany(c => c.Selectors))
            {
                if (selector.AttributeRouteModel != null)
                {
                    selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(
                        this._routePrefix,
                        selector.AttributeRouteModel);
                }
                else
                {
                    selector.AttributeRouteModel = this._routePrefix;
                }
            }
        }
    }
}
