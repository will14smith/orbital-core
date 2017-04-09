﻿using System;
using System.Collections.Generic;
using Halcyon.HAL;
using Halcyon.Web.HAL;
using Microsoft.AspNetCore.Mvc;

namespace Orbital.Web.Helpers
{
    public static class ControllerExtensions
    {
        public static IActionResult Paginate<T>(this Controller controller, IReadOnlyCollection<T> data, string dataName, int page, int pageSize = 100000)
        {
            // TODO items displayed on page
            var count = data.Count;
            // TODO  total number of items
            var total = data.Count;

            var navLinks = controller.Url.PaginationLinks(page, pageSize, total);

            var response = new HALResponse(new { count, total })
                .AddSelfLink(controller.Request)
                .AddLinks(navLinks)
                .AddEmbeddedCollection(dataName, data);

            return controller.Ok(response);
        }

        public static IEnumerable<Link> PaginationLinks(this IUrlHelper url, int page, int pageSize, int totalItemCount)
        {
            var totalPages = (int)Math.Ceiling((decimal)totalItemCount / pageSize);

            yield return new Link("first", url.Action("Get", new { page = 1 }));
            if (page > 1)
            {
                yield return new Link("prev", url.Action("Get", new { page = page - 1 }));
            }
            if (page < totalPages)
            {
                yield return new Link("next", url.Action("Get", new { page = page + 1 }));
            }
            yield return new Link("last", url.Action("Get", new { page = totalPages }));
        }

    }
}
