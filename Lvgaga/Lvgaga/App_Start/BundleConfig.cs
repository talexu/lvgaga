﻿using System.Web.Optimization;

namespace Lvgaga
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/bower_components/jquery-ajax-retry/dist/jquery.ajax-retry*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jquerylazy").Include(
                "~/Scripts/jquery.lazyload*"));

            bundles.Add(new ScriptBundle("~/bundles/gallery").Include(
                        "~/bower_components/blueimp-gallery/js/jquery.blueimp-gallery.min.js"));

            // 业务逻辑
            bundles.Add(new ScriptBundle("~/bundles/business").Include(
                "~/Scripts/utilities.js"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css",
                "~/Content/lvgaga.css",
                "~/Content/ladda-themeless.css"));

            bundles.Add(new StyleBundle("~/Content/css/gallery").Include(
                "~/bower_components/blueimp-gallery/css/blueimp-gallery.min.css"));

            #region product
            // Layout Page
            bundles.Add(new ScriptBundle("~/bundles/layout").Include(
                "~/Scripts/jquery-{version}.js",
                "~/bower_components/jquery-ajax-retry/dist/jquery.ajax-retry*",
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js",
                "~/Scripts/spin*",
                "~/Scripts/ladda*",
                "~/Scripts/moment*",
                "~/bower_components/sprintf/dist/sprintf.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/utilities").Include(
                "~/Scripts/lv.utility.js"));

            // Home Page
            bundles.Add(new ScriptBundle("~/bundles/home").Include(
                "~/Scripts/jquery.lazyload*",
                "~/Scripts/lv.tumblr.js"));

            // Comment Page
            bundles.Add(new ScriptBundle("~/bundles/comment").Include(
                "~/bower_components/sisyphus/sisyphus*",
                "~/Scripts/lv.comment.js"));

            // Favorite Page
            bundles.Add(new ScriptBundle("~/bundles/favorite").Include(
                "~/bower_components/blueimp-gallery/js/jquery.blueimp-gallery.min.js",
                "~/Scripts/lv.favorite.js"));
            #endregion
        }
    }
}
