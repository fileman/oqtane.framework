﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure.Interfaces;
using Oqtane.Models;
using Oqtane.Modules;
using Oqtane.Shared;
using Module = Oqtane.Models.Module;

namespace Oqtane.Repository
{
    public class SiteRepository : ISiteRepository
    {
        private readonly TenantDBContext _db;
        private readonly IRoleRepository _roleRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IFolderRepository _folderRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IPageRepository _pageRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IPageModuleRepository _pageModuleRepository;
        private readonly IModuleDefinitionRepository _moduleDefinitionRepository;
        private readonly IPermissionRepository _permissionRepository;

        private readonly IServiceProvider _serviceProvider;

        private readonly IConfigurationRoot _config;

        public SiteRepository(TenantDBContext context, IRoleRepository roleRepository, IProfileRepository profileRepository, IFolderRepository folderRepository, IFileRepository fileRepository, IPageRepository pageRepository,
            IModuleRepository moduleRepository, IPageModuleRepository pageModuleRepository, IModuleDefinitionRepository moduleDefinitionRepository, IPermissionRepository permissionRepository, IServiceProvider serviceProvider,
            IConfigurationRoot config)
        {
            _db = context;
            _roleRepository = roleRepository;
            _profileRepository = profileRepository;
            _folderRepository = folderRepository;
            _fileRepository = fileRepository;
            _pageRepository = pageRepository;
            _moduleRepository = moduleRepository;
            _pageModuleRepository = pageModuleRepository;
            _moduleDefinitionRepository = moduleDefinitionRepository;
            _permissionRepository = permissionRepository;
            _serviceProvider = serviceProvider;
            _config = config;
        }

        private List<PageTemplate> CreateAdminPages(List<PageTemplate> pageTemplates = null)
        {
            if (pageTemplates == null) pageTemplates = new List<PageTemplate>();

            pageTemplates.Add(new PageTemplate
            {
                Name = "Admin", Parent = "", Path = "admin", Icon = "", IsNavigation = false, IsPersonalizable = false, EditMode = true,
                PagePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                {
                    new Permission(PermissionNames.View, Constants.AdminRole, true),
                    new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                }),
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = "Oqtane.Modules.Admin.Dashboard, Oqtane.Client", Title = "Admin Dashboard", Pane = "Content",
                        ModulePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                        {
                            new Permission(PermissionNames.View, Constants.AdminRole, true),
                            new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                        }),
                        Content = ""
                    }
                }
            });
            pageTemplates.Add(new PageTemplate
            {
                Name = "Site Management", Parent = "Admin", Path = "admin/sites", Icon = "globe", IsNavigation = false, IsPersonalizable = false, EditMode = true,
                PagePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                {
                    new Permission(PermissionNames.View, Constants.AdminRole, true),
                    new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                }),
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = "Oqtane.Modules.Admin.Sites, Oqtane.Client", Title = "Site Management", Pane = "Content",
                        ModulePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                        {
                            new Permission(PermissionNames.View, Constants.AdminRole, true),
                            new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                        }),
                        Content = ""
                    }
                }
            });
            pageTemplates.Add(new PageTemplate
            {
                Name = "Site Settings", Parent = "Admin", Path = "admin/site", Icon = "home", IsNavigation = false, IsPersonalizable = false, EditMode = true,
                PagePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                {
                    new Permission(PermissionNames.View, Constants.AdminRole, true),
                    new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                }),
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = "Oqtane.Modules.Admin.Site, Oqtane.Client", Title = "Site Settings", Pane = "Content",
                        ModulePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                        {
                            new Permission(PermissionNames.View, Constants.AdminRole, true),
                            new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                        }),
                        Content = ""
                    }
                }
            });
            pageTemplates.Add(new PageTemplate
            {
                Name = "Page Management", Parent = "Admin", Path = "admin/pages", Icon = "layers", IsNavigation = false, IsPersonalizable = false, EditMode = true,
                PagePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                {
                    new Permission(PermissionNames.View, Constants.AdminRole, true),
                    new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                }),
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = "Oqtane.Modules.Admin.Pages, Oqtane.Client", Title = "Page Management", Pane = "Content",
                        ModulePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                        {
                            new Permission(PermissionNames.View, Constants.AdminRole, true),
                            new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                        }),
                        Content = ""
                    }
                }
            });
            pageTemplates.Add(new PageTemplate
            {
                Name = "User Management", Parent = "Admin", Path = "admin/users", Icon = "people", IsNavigation = false, IsPersonalizable = false, EditMode = true,
                PagePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                {
                    new Permission(PermissionNames.View, Constants.AdminRole, true),
                    new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                }),
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = "Oqtane.Modules.Admin.Users, Oqtane.Client", Title = "User Management", Pane = "Content",
                        ModulePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                        {
                            new Permission(PermissionNames.View, Constants.AdminRole, true),
                            new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                        }),
                        Content = ""
                    }
                }
            });
            pageTemplates.Add(new PageTemplate
            {
                Name = "Profile Management", Parent = "Admin", Path = "admin/profiles", Icon = "person", IsNavigation = false, IsPersonalizable = false, EditMode = true,
                PagePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                {
                    new Permission(PermissionNames.View, Constants.AdminRole, true),
                    new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                }),
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = "Oqtane.Modules.Admin.Profiles, Oqtane.Client", Title = "Profile Management", Pane = "Content",
                        ModulePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                        {
                            new Permission(PermissionNames.View, Constants.AdminRole, true),
                            new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                        }),
                        Content = ""
                    }
                }
            });
            pageTemplates.Add(new PageTemplate
            {
                Name = "Role Management", Parent = "Admin", Path = "admin/roles", Icon = "lock-locked", IsNavigation = false, IsPersonalizable = false, EditMode = true,
                PagePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                {
                    new Permission(PermissionNames.View, Constants.AdminRole, true),
                    new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                }),
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = "Oqtane.Modules.Admin.Roles, Oqtane.Client", Title = "Role Management", Pane = "Content",
                        ModulePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                        {
                            new Permission(PermissionNames.View, Constants.AdminRole, true),
                            new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                        }),
                        Content = ""
                    }
                }
            });
            pageTemplates.Add(new PageTemplate
            {
                Name = "Event Log", Parent = "Admin", Path = "admin/log", Icon = "magnifying-glass", IsNavigation = false, IsPersonalizable = false, EditMode = true,
                PagePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                {
                    new Permission(PermissionNames.View, Constants.AdminRole, true),
                    new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                }),
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = "Oqtane.Modules.Admin.Logs, Oqtane.Client", Title = "Event Log", Pane = "Content",
                        ModulePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                        {
                            new Permission(PermissionNames.View, Constants.AdminRole, true),
                            new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                        }),
                        Content = ""
                    }
                }
            });
            pageTemplates.Add(new PageTemplate
            {
                Name = "File Management", Parent = "Admin", Path = "admin/files", Icon = "file", IsNavigation = false, IsPersonalizable = false, EditMode = true,
                PagePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                {
                    new Permission(PermissionNames.View, Constants.AdminRole, true),
                    new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                }),
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = "Oqtane.Modules.Admin.Files, Oqtane.Client", Title = "File Management", Pane = "Content",
                        ModulePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                        {
                            new Permission(PermissionNames.View, Constants.AdminRole, true),
                            new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                        }),
                        Content = ""
                    }
                }
            });
            pageTemplates.Add(new PageTemplate
            {
                Name = "Recycle Bin", Parent = "Admin", Path = "admin/recyclebin", Icon = "trash", IsNavigation = false, IsPersonalizable = false, EditMode = true,
                PagePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                {
                    new Permission(PermissionNames.View, Constants.AdminRole, true),
                    new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                }),
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = "Oqtane.Modules.Admin.RecycleBin, Oqtane.Client", Title = "Recycle Bin", Pane = "Content",
                        ModulePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                        {
                            new Permission(PermissionNames.View, Constants.AdminRole, true),
                            new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                        }),
                        Content = ""
                    }
                }
            });
            pageTemplates.Add(new PageTemplate
            {
                Name = "Tenant Management", Parent = "Admin", Path = "admin/tenants", Icon = "list", IsNavigation = false, IsPersonalizable = false, EditMode = true,
                PagePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                {
                    new Permission(PermissionNames.View, Constants.AdminRole, true),
                    new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                }),
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = "Oqtane.Modules.Admin.Tenants, Oqtane.Client", Title = "Tenant Management", Pane = "Content",
                        ModulePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                        {
                            new Permission(PermissionNames.View, Constants.AdminRole, true),
                            new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                        }),
                        Content = ""
                    }
                }
            });
            pageTemplates.Add(new PageTemplate
            {
                Name = "Module Management", Parent = "Admin", Path = "admin/modules", Icon = "browser", IsNavigation = false, IsPersonalizable = false, EditMode = true,
                PagePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                {
                    new Permission(PermissionNames.View, Constants.AdminRole, true),
                    new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                }),
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = "Oqtane.Modules.Admin.ModuleDefinitions, Oqtane.Client", Title = "Module Management", Pane = "Content",
                        ModulePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                        {
                            new Permission(PermissionNames.View, Constants.AdminRole, true),
                            new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                        }),
                        Content = ""
                    }
                }
            });
            pageTemplates.Add(new PageTemplate
            {
                Name = "Theme Management", Parent = "Admin", Path = "admin/themes", Icon = "brush", IsNavigation = false, IsPersonalizable = false, EditMode = true,
                PagePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                {
                    new Permission(PermissionNames.View, Constants.AdminRole, true),
                    new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                }),
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = "Oqtane.Modules.Admin.Themes, Oqtane.Client", Title = "Theme Management", Pane = "Content",
                        ModulePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                        {
                            new Permission(PermissionNames.View, Constants.AdminRole, true),
                            new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                        }),
                        Content = ""
                    }
                }
            });
            pageTemplates.Add(new PageTemplate
            {
                Name = "Scheduled Jobs", Parent = "Admin", Path = "admin/jobs", Icon = "timer", IsNavigation = false, IsPersonalizable = false, EditMode = true,
                PagePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                {
                    new Permission(PermissionNames.View, Constants.AdminRole, true),
                    new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                }),
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = "Oqtane.Modules.Admin.Jobs, Oqtane.Client", Title = "Scheduled Jobs", Pane = "Content",
                        ModulePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                        {
                            new Permission(PermissionNames.View, Constants.AdminRole, true),
                            new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                        }),
                        Content = ""
                    }
                }
            });
            pageTemplates.Add(new PageTemplate
            {
                Name = "Sql Management",
                Parent = "Admin",
                Path = "admin/sql",
                Icon = "spreadsheet",
                IsNavigation = false,
                IsPersonalizable = false,
                EditMode = true,
                PagePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                {
                    new Permission(PermissionNames.View, Constants.AdminRole, true),
                    new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                }),
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = "Oqtane.Modules.Admin.Sql, Oqtane.Client", Title = "Sql Management", Pane = "Content",
                        ModulePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                        {
                            new Permission(PermissionNames.View, Constants.AdminRole, true),
                            new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                        }),
                        Content = ""
                    }
                }
            });
            pageTemplates.Add(new PageTemplate
            {
                Name = "Upgrade Service", Parent = "Admin", Path = "admin/upgrade", Icon = "aperture", IsNavigation = false, IsPersonalizable = false, EditMode = true,
                PagePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                {
                    new Permission(PermissionNames.View, Constants.AdminRole, true),
                    new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                }),
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = "Oqtane.Modules.Admin.Upgrade, Oqtane.Client", Title = "Upgrade Service", Pane = "Content",
                        ModulePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                        {
                            new Permission(PermissionNames.View, Constants.AdminRole, true),
                            new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                        }),
                        Content = ""
                    }
                }
            });
            pageTemplates.Add(new PageTemplate
            {
                Name = "Login", Parent = "", Path = "login", Icon = "lock-locked", IsNavigation = false, IsPersonalizable = false, EditMode = false,
                PagePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                {
                    new Permission(PermissionNames.View, Constants.AdminRole, true),
                    new Permission(PermissionNames.View, Constants.AllUsersRole, true),
                    new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                }),
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = "Oqtane.Modules.Admin.Login, Oqtane.Client", Title = "User Login", Pane = "Content",
                        ModulePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                        {
                            new Permission(PermissionNames.View, Constants.AdminRole, true),
                            new Permission(PermissionNames.View, Constants.AllUsersRole, true),
                            new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                        }),
                        Content = ""
                    }
                }
            });
            pageTemplates.Add(new PageTemplate
            {
                Name = "Register", Parent = "", Path = "register", Icon = "person", IsNavigation = false, IsPersonalizable = false, EditMode = false,
                PagePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                {
                    new Permission(PermissionNames.View, Constants.AdminRole, true),
                    new Permission(PermissionNames.View, Constants.AllUsersRole, true),
                    new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                }),
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = "Oqtane.Modules.Admin.Register, Oqtane.Client", Title = "User Registration", Pane = "Content",
                        ModulePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                        {
                            new Permission(PermissionNames.View, Constants.AdminRole, true),
                            new Permission(PermissionNames.View, Constants.AllUsersRole, true),
                            new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                        }),
                        Content = ""
                    }
                }
            });

            pageTemplates.Add(new PageTemplate
            {
                Name = "Reset", Parent = "", Path = "reset", Icon = "person", IsNavigation = false, IsPersonalizable = false, EditMode = false,
                PagePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                {
                    new Permission(PermissionNames.View, Constants.AdminRole, true),
                    new Permission(PermissionNames.View, Constants.AllUsersRole, true),
                    new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                }),
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = "Oqtane.Modules.Admin.Reset, Oqtane.Client", Title = "Password Reset", Pane = "Content",
                        ModulePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                        {
                            new Permission(PermissionNames.View, Constants.AdminRole, true),
                            new Permission(PermissionNames.View, Constants.AllUsersRole, true),
                            new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                        }),
                        Content = ""
                    }
                }
            });
            pageTemplates.Add(new PageTemplate
            {
                Name = "Profile", Parent = "", Path = "profile", Icon = "person", IsNavigation = false, IsPersonalizable = false, EditMode = false,
                PagePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                {
                    new Permission(PermissionNames.View, Constants.AdminRole, true),
                    new Permission(PermissionNames.View, Constants.RegisteredRole, true),
                    new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                }),
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = "Oqtane.Modules.Admin.UserProfile, Oqtane.Client", Title = "User Profile", Pane = "Content",
                        ModulePermissions = _permissionRepository.EncodePermissions(new List<Permission>
                        {
                            new Permission(PermissionNames.View, Constants.AdminRole, true),
                            new Permission(PermissionNames.View, Constants.RegisteredRole, true),
                            new Permission(PermissionNames.Edit, Constants.AdminRole, true)
                        }),
                        Content = ""
                    }
                }
            });
            return pageTemplates;
        }

        public IEnumerable<Site> GetSites()
        {
            return _db.Site;
        }

        public Site AddSite(Site site)
        {
            _db.Site.Add(site);
            _db.SaveChanges();
            CreateSite(site);
            return site;
        }

        public Site UpdateSite(Site site)
        {
            _db.Entry(site).State = EntityState.Modified;
            _db.SaveChanges();
            return site;
        }

        public Site GetSite(int siteId)
        {
            return _db.Site.Find(siteId);
        }

        public void DeleteSite(int siteId)
        {
            var site = _db.Site.Find(siteId);
            _db.Site.Remove(site);
            _db.SaveChanges();
        }

        private void CreateSite(Site site)
        {
            // create default entities for site
            List<Role> roles = _roleRepository.GetRoles(site.SiteId, true).ToList();
            if (!roles.Where(item => item.Name == Constants.AllUsersRole).Any())
            {
                _roleRepository.AddRole(new Role {SiteId = null, Name = Constants.AllUsersRole, Description = "All Users", IsAutoAssigned = false, IsSystem = true});
            }

            if (!roles.Where(item => item.Name == Constants.HostRole).Any())
            {
                _roleRepository.AddRole(new Role {SiteId = null, Name = Constants.HostRole, Description = "Application Administrators", IsAutoAssigned = false, IsSystem = true});
            }

            _roleRepository.AddRole(new Role {SiteId = site.SiteId, Name = Constants.RegisteredRole, Description = "Registered Users", IsAutoAssigned = true, IsSystem = true});
            _roleRepository.AddRole(new Role {SiteId = site.SiteId, Name = Constants.AdminRole, Description = "Site Administrators", IsAutoAssigned = false, IsSystem = true});

            _profileRepository.AddProfile(new Profile
                {SiteId = site.SiteId, Name = "FirstName", Title = "First Name", Description = "Your First Or Given Name", Category = "Name", ViewOrder = 1, MaxLength = 50, DefaultValue = "", IsRequired = true, IsPrivate = false});
            _profileRepository.AddProfile(new Profile
                {SiteId = site.SiteId, Name = "LastName", Title = "Last Name", Description = "Your Last Or Family Name", Category = "Name", ViewOrder = 2, MaxLength = 50, DefaultValue = "", IsRequired = true, IsPrivate = false});
            _profileRepository.AddProfile(new Profile
                {SiteId = site.SiteId, Name = "Street", Title = "Street", Description = "Street Or Building Address", Category = "Address", ViewOrder = 3, MaxLength = 50, DefaultValue = "", IsRequired = false, IsPrivate = false});
            _profileRepository.AddProfile(
                new Profile {SiteId = site.SiteId, Name = "City", Title = "City", Description = "City", Category = "Address", ViewOrder = 4, MaxLength = 50, DefaultValue = "", IsRequired = false, IsPrivate = false});
            _profileRepository.AddProfile(new Profile
                {SiteId = site.SiteId, Name = "Region", Title = "Region", Description = "State Or Province", Category = "Address", ViewOrder = 5, MaxLength = 50, DefaultValue = "", IsRequired = false, IsPrivate = false});
            _profileRepository.AddProfile(new Profile
                {SiteId = site.SiteId, Name = "Country", Title = "Country", Description = "Country", Category = "Address", ViewOrder = 6, MaxLength = 50, DefaultValue = "", IsRequired = false, IsPrivate = false});
            _profileRepository.AddProfile(new Profile
                {SiteId = site.SiteId, Name = "PostalCode", Title = "Postal Code", Description = "Postal Code Or Zip Code", Category = "Address", ViewOrder = 7, MaxLength = 50, DefaultValue = "", IsRequired = false, IsPrivate = false});
            _profileRepository.AddProfile(new Profile
                {SiteId = site.SiteId, Name = "Phone", Title = "Phone Number", Description = "Phone Number", Category = "Contact", ViewOrder = 8, MaxLength = 50, DefaultValue = "", IsRequired = false, IsPrivate = false});

            Folder folder = _folderRepository.AddFolder(new Folder
            {
                SiteId = site.SiteId, ParentId = null, Name = "Root", Path = "", Order = 1, IsSystem = true,
                Permissions = "[{\"PermissionName\":\"Browse\",\"Permissions\":\"Administrators\"},{\"PermissionName\":\"View\",\"Permissions\":\"All Users\"},{\"PermissionName\":\"Edit\",\"Permissions\":\"Administrators\"}]"
            });
            _folderRepository.AddFolder(new Folder
            {
                SiteId = site.SiteId, ParentId = folder.FolderId, Name = "Users", Path = "Users\\", Order = 1, IsSystem = true,
                Permissions = "[{\"PermissionName\":\"Browse\",\"Permissions\":\"Administrators\"},{\"PermissionName\":\"View\",\"Permissions\":\"Administrators\"},{\"PermissionName\":\"Edit\",\"Permissions\":\"Administrators\"}]"
            });

            var _pageTemplates = CreateAdminPages();
            CreatePages(site, _pageTemplates);

            // process site template
            if (string.IsNullOrEmpty(site.SiteTemplateType))
            {
                var section = _config.GetSection("SiteTemplate");
                if (section.Exists())
                {
                    site.SiteTemplateType = section.Value;
                }
                else
                {
                    site.SiteTemplateType = Constants.DefaultSiteTemplate;
                }
            }

            Type siteTemplateType = Type.GetType(site.SiteTemplateType);
            if (siteTemplateType != null)
            {
                var siteTemplateObject = ActivatorUtilities.CreateInstance(_serviceProvider, siteTemplateType);
                List<PageTemplate> pageTemplates = ((ISiteTemplate) siteTemplateObject).CreateSite(site);
                if (pageTemplates != null && pageTemplates.Count > 0)
                {
                    CreatePages(site, pageTemplates);
                }
            }
        }

        private void CreatePages(Site site, List<PageTemplate> pageTemplates)
        {
            List<ModuleDefinition> moduledefinitions = _moduleDefinitionRepository.GetModuleDefinitions(site.SiteId).ToList();
            foreach (PageTemplate pagetemplate in pageTemplates)
            {
                int? parentid = null;
                if (pagetemplate.Parent != "")
                {
                    List<Page> pages = _pageRepository.GetPages(site.SiteId).ToList();
                    Page parent = pages.Where(item => item.Name == pagetemplate.Parent).FirstOrDefault();
                    parentid = parent.PageId;
                }

                Page page = new Page
                {
                    SiteId = site.SiteId,
                    ParentId = parentid,
                    Name = pagetemplate.Name,
                    Path = pagetemplate.Path,
                    Order = 1,
                    IsNavigation = pagetemplate.IsNavigation,
                    EditMode = pagetemplate.EditMode,
                    ThemeType = "",
                    LayoutType = "",
                    Icon = pagetemplate.Icon,
                    Permissions = pagetemplate.PagePermissions,
                    IsPersonalizable = pagetemplate.IsPersonalizable,
                    UserId = null
                };
                page = _pageRepository.AddPage(page);

                foreach (PageTemplateModule pagetemplatemodule in pagetemplate.PageTemplateModules)
                {
                    if (pagetemplatemodule.ModuleDefinitionName != "")
                    {
                        ModuleDefinition moduledefinition = moduledefinitions.Where(item => item.ModuleDefinitionName == pagetemplatemodule.ModuleDefinitionName).FirstOrDefault();
                        if (moduledefinition != null)
                        {
                            Module module = new Module
                            {
                                SiteId = site.SiteId,
                                ModuleDefinitionName = pagetemplatemodule.ModuleDefinitionName,
                                Permissions = pagetemplatemodule.ModulePermissions,
                            };
                            module = _moduleRepository.AddModule(module);

                            if (pagetemplatemodule.Content != "" && moduledefinition.ServerAssemblyName != "")
                            {
                                Assembly assembly = AppDomain.CurrentDomain.GetAssemblies()
                                    .Where(item => item.FullName.StartsWith(moduledefinition.ServerAssemblyName)).FirstOrDefault();
                                if (assembly != null)
                                {
                                    Type moduletype = assembly.GetTypes()
                                        .Where(item => item.Namespace != null)
                                        .Where(item => item.Namespace.StartsWith(moduledefinition.ModuleDefinitionName.Substring(0, moduledefinition.ModuleDefinitionName.IndexOf(","))))
                                        .Where(item => item.GetInterfaces().Contains(typeof(IPortable))).FirstOrDefault();
                                    if (moduletype != null)
                                    {
                                        var moduleobject = ActivatorUtilities.CreateInstance(_serviceProvider, moduletype);
                                        ((IPortable) moduleobject).ImportModule(module, pagetemplatemodule.Content, moduledefinition.Version);
                                    }
                                }
                            }

                            PageModule pagemodule = new PageModule
                            {
                                PageId = page.PageId,
                                ModuleId = module.ModuleId,
                                Title = pagetemplatemodule.Title,
                                Pane = pagetemplatemodule.Pane,
                                Order = 1,
                                ContainerType = ""
                            };
                            _pageModuleRepository.AddPageModule(pagemodule);
                        }
                    }
                }
            }
        }
    }
}
