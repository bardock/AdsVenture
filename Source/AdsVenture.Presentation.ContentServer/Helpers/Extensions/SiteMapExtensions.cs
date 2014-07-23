using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;

namespace AdsVenture.Presentation.ContentServer.Helpers.Extensions
{
    public static class SiteMapExtensions
    {
	    public const string TITLE_SEPARATOR = " > ";

	    public static string GetViewTitle(this HtmlHelper helper)
	    {
            ISiteMapNode node = SiteMaps.Current.CurrentNode;
		    string title = string.Empty;

		    if (node == null) {
			    //Si la pagina no está en el sitemap
                return SiteMaps.Current.RootNode.Title;
		    }
		    //Generar un titulo compuesto por los titulos de los nodos en la ascendencia de la pagina actual
            title = helper.ViewBag.AddPathNodeWithTitle  ?? string.Empty;
		    while (node != null && node != SiteMaps.Current.RootNode) {
			    title = node.Title + (title.Length > 0 ? TITLE_SEPARATOR : string.Empty) + title;
			    node = node.ParentNode;
		    }
		    return title;
	    }
    }
}