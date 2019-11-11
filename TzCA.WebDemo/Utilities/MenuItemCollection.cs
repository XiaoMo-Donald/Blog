﻿using TzCA.Common.ViewModelComponents;
using TzCA.DataAccess.SqlServer;
using TzCA.Entities.ApplicationOrganization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TzCA.Web.Utilities
{
    /// <summary>
    /// 用于存储系统相关导航菜单的静态类
    /// </summary>
    public static class MenuItemCollection
    {
        private static EntityDbContext _Context;
        private static List<SimpleMainTopMenuItem> _MainTopMenuItems;  // 用于后台管理的主菜单条目集合
        private static List<SimpleSubMenuItem> _SubMenuItems;          // 用于对应后台管理的左侧菜单条目集合

        public static Guid CurrentMainTopMenuItemId { get; set; }  // 当前使用的主菜单条目的 Id

        /// <summary>
        /// 菜单数据初始化处理
        /// </summary>
        /// <param name="context"></param>
        public static void Initializer(EntityDbContext context)
        {
            _Context = context;
            if(_MainTopMenuItems==null)
                _SetMainTopMenuItems();
            if(_SubMenuItems==null)
                _SetSubMenuItems();

            CurrentMainTopMenuItemId = _MainTopMenuItems.FirstOrDefault().Id;
        }

        /// <summary>
        /// 提取 SystemWorkPlace 数据初始化用于后台管理的主菜单条目集合
        /// </summary>
        private static void _SetMainTopMenuItems()
        {
            var menuItems = new List<SimpleMainTopMenuItem>();
            foreach(var item in _Context.SystemWorkPlaces.OrderBy(x=>x.SortCode))
            {
                menuItems.Add(new SimpleMainTopMenuItem() { Id=item.Id, Name=item.Name,URL=item.URL,SortCode=item.SortCode });
            }
            _MainTopMenuItems = menuItems;
        }

        /// <summary>
        /// 返回后台管理的主菜单条目集合
        /// </summary>
        /// <returns></returns>
        public static List<SimpleMainTopMenuItem> GetMainTopMenuItem()
        {
            return _MainTopMenuItems;
        }

        /// <summary>
        /// 更新后台管理的主菜单条目
        /// </summary>
        /// <param name="wp"> SystemWorkPlace 实例对象 </param>
        public static void UpdateMainTopMenuItem(SystemWorkPlace wp)
        {
            var menuItem = _MainTopMenuItems.FirstOrDefault(x => x.Id == wp.Id);
            if (menuItem == null)
            {
                _MainTopMenuItems.Add(new SimpleMainTopMenuItem() { Id = wp.Id, Name = wp.Name, URL = wp.URL,SortCode=wp.SortCode });
            }
            else
            {
                menuItem.Name = wp.Name;
                menuItem.URL = wp.URL;
                menuItem.SortCode = wp.SortCode;
            }
        }

        /// <summary>
        /// 提取 SystemWorkPlace 以及 SystemWorkTask 数据初始化合成用于后台管理的主菜单条目集合
        /// </summary>
        private static void _SetSubMenuItems()
        {
            _SubMenuItems = new List<SimpleSubMenuItem>();
            var wpItems = _Context.SystemWorkPlaces;//.SelectMany(x => x.SystemWorkSections);
            foreach(var item in wpItems)
            {
                _Context.Entry(item).Collection(x => x.SystemWorkSections).Load();
                foreach(var sItem in item.SystemWorkSections)
                {
                    var subMenuItem = new SimpleSubMenuItem() { Id = sItem.Id, Name = sItem.Name, SortCode = sItem.SortCode, ParentId = item.Id, URL = "" };
                    _SubMenuItems.Add(subMenuItem);
                    _Context.Entry(sItem).Collection(x => x.SystemWorkTasks).Load();
                    foreach(var mItem in sItem.SystemWorkTasks)
                    {
                        var miniMenuItem = new SimpleSubMenuItem()
                        {
                            Id = mItem.Id,
                            Name = mItem.Name,
                            URL = "../../" + mItem.ControllerName,
                            SortCode = mItem.SortCode,
                            ParentId = sItem.Id
                        };
                        _SubMenuItems.Add(miniMenuItem);
                    }
                }
            }
        }

        /// <summary>
        /// 根据归属的主菜单元素的 id 返回对应后台管理的左侧菜单条目集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<SimpleSubMenuItem> GetSubmenuItems(Guid id)
        {
            var result = new List<SimpleSubMenuItem>();
            var sItems = _SubMenuItems.Where(x => x.ParentId == id).OrderBy(y=>y.SortCode);
            foreach(var sItem in sItems)
            {
                result.Add(sItem);
                var mItems = _SubMenuItems.Where(x => x.ParentId == sItem.Id).OrderBy(y => y.SortCode);
                foreach(var mItem in mItems)
                {
                    result.Add(mItem);
                }
            }
            return result;
        }

        /// <summary>
        /// 根据 SystemWorkSection 和相应的归属 Id 对象，更新左侧菜单条目
        /// </summary>
        /// <param name="bo">SystemWorkSection 对象</param>
        /// <param name="pId">归属主菜单的Id</param>
        public static void UpdateSubMenuItems(SystemWorkSection bo,Guid pId)
        {
            var sMenuItem = _SubMenuItems.FirstOrDefault(x => x.Id == bo.Id);
            if (sMenuItem == null)
            {
                var miniMenuItem = new SimpleSubMenuItem()
                {
                    Id = bo.Id,
                    Name = bo.Name,
                    URL = "",
                    SortCode = bo.SortCode,
                    ParentId = pId
                };
                _SubMenuItems.Add(miniMenuItem);
            }
            else
            {
                sMenuItem.Name = bo.Name;
                sMenuItem.URL = "";
                sMenuItem.SortCode = bo.SortCode;
                sMenuItem.ParentId = pId;
            }
        }

        /// <summary>
        /// 根据 SystemWorkTask 和相应的归属 Id 对象，更新左侧菜单条目
        /// </summary>
        /// <param name="bo">SystemWorkTask 对象</param>
        /// <param name="pId">归属对象Id</param>
        public static void UpdateSubMenuItems(SystemWorkTask bo, Guid pId)
        {
            var sMenuItem = _SubMenuItems.FirstOrDefault(x => x.Id == bo.Id);
            if (sMenuItem == null)
            {
                var miniMenuItem = new SimpleSubMenuItem()
                {
                    Id = bo.Id,
                    Name = bo.Name,
                    URL = "../../" + bo.ControllerName,
                    SortCode = bo.SortCode,
                    ParentId = pId
                };
                _SubMenuItems.Add(miniMenuItem);
            }
            else
            {
                sMenuItem.Name = bo.Name;
                sMenuItem.URL = "../../" + bo.ControllerName;
                sMenuItem.SortCode = bo.SortCode;
                sMenuItem.ParentId = pId;
            }
        }
    }
}
