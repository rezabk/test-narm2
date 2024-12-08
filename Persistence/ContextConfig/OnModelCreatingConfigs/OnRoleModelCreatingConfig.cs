namespace Persistence.ContextConfig.OnModelCreatingConfigs;
//public class OnRoleModelCreatingConfig : IEntityTypeConfiguration<Role>
//{
//    public void Configure(EntityTypeBuilder<Role> builder)
//    {
//        builder.HasData(new Role
//        {
//            Id = RoleEnum.AboutUs.GetId(),
//            Name = Enum.GetName(RoleEnum.AboutUs),
//            NormalizedName = Enum.GetName(RoleEnum.AboutUs)?.Normalize(),
//            DisplayName = RoleEnum.AboutUs.GetDescription()
//        });
//        builder.HasData(new Role
//        {
//            Id = RoleEnum.CmsSetting.GetId(),
//            Name = Enum.GetName(RoleEnum.CmsSetting),
//            NormalizedName = Enum.GetName(RoleEnum.CmsSetting).Normalize(),
//            DisplayName = RoleEnum.CmsSetting.GetDescription()
//        });
//        builder.HasData(new Role
//        {
//            Id = RoleEnum.ContactUs.GetId(),
//            Name= Enum.GetName(RoleEnum.ContactUs),
//            NormalizedName = Enum.GetName(RoleEnum.ContactUs).Normalize(),
//            DisplayName = RoleEnum.ContactUs.GetDescription()
//        });
//        builder.HasData(new Role
//        {
//            Id = RoleEnum.Gallery.GetId(),
//            Name = Enum.GetName(RoleEnum.Gallery),
//            NormalizedName = Enum.GetName(RoleEnum.Gallery).Normalize(),
//            DisplayName = RoleEnum.Gallery.GetDescription()
//        });
//        builder.HasData(new Role
//        {
//            Id = RoleEnum.Menu.GetId(),
//            Name = Enum.GetName(RoleEnum.Menu),
//            NormalizedName = Enum.GetName(RoleEnum.Menu).Normalize(),
//            DisplayName = RoleEnum.Menu.GetDescription()

//        });
//        builder.HasData(new Role
//        {
//            Id = RoleEnum.Slider.GetId(),
//            Name = Enum.GetName(RoleEnum.Slider),
//            NormalizedName = Enum.GetName(RoleEnum.Slider).Normalize(),
//            DisplayName = RoleEnum.Slider.GetDescription()
//        });
//        builder.HasData(new Role
//        {
//            Id = RoleEnum.ServiceDesk.GetId(),
//            Name = Enum.GetName(RoleEnum.ServiceDesk),
//            NormalizedName = Enum.GetName(RoleEnum.ServiceDesk).Normalize(),
//            DisplayName = RoleEnum.ServiceDesk.GetDescription()
//        });
//        builder.HasData(new Role
//        {
//            Id = RoleEnum.Notification.GetId(),
//            Name = Enum.GetName(RoleEnum.Notification),
//            NormalizedName = Enum.GetName(RoleEnum.Notification).Normalize(),
//            DisplayName = RoleEnum.Notification.GetDescription()
//        });
//        builder.HasData(new Role
//        {
//            Id = RoleEnum.Statement.GetId(),
//            Name = Enum.GetName(RoleEnum.Statement),
//            NormalizedName = Enum.GetName(RoleEnum.Statement).Normalize(),
//            DisplayName = RoleEnum.Statement.GetDescription()
//        });
//        builder.HasData(new Role
//        {
//            Id = RoleEnum.QuickAccess.GetId(),
//            Name = Enum.GetName(RoleEnum.QuickAccess),
//            NormalizedName = Enum.GetName(RoleEnum.QuickAccess).Normalize(),
//            DisplayName = RoleEnum.QuickAccess.GetDescription()
//        });
//        builder.HasData(new Role
//        {
//            Id = RoleEnum.RelatedLink.GetId(),
//            Name = Enum.GetName(RoleEnum.RelatedLink),
//            NormalizedName = Enum.GetName(RoleEnum.RelatedLink).Normalize(),
//            DisplayName = RoleEnum.RelatedLink.GetDescription()
//        });
//        builder.HasData(new Role
//        {
//            Id = RoleEnum.TextNews.GetId(),
//            Name = Enum.GetName(RoleEnum.TextNews),
//            NormalizedName = Enum.GetName(RoleEnum.TextNews).Normalize(),
//            DisplayName = RoleEnum.TextNews.GetDescription()
//        });
//        builder.HasData(new Role
//        {
//            Id = RoleEnum.ImageNews.GetId(),
//            Name = Enum.GetName(RoleEnum.ImageNews),
//            NormalizedName = Enum.GetName(RoleEnum.ImageNews).Normalize(),
//            DisplayName = RoleEnum.ImageNews.GetDescription()
//        });
//        builder.HasData(new Role
//        {
//            Id = RoleEnum.VideoNews.GetId(),
//            Name = Enum.GetName(RoleEnum.VideoNews),
//            NormalizedName = Enum.GetName(RoleEnum.VideoNews).Normalize(),
//            DisplayName = RoleEnum.VideoNews.GetDescription()
//        });
//        builder.HasData(new Role
//        {
//            Id = RoleEnum.UserManagement.GetId(),
//            Name = Enum.GetName(RoleEnum.UserManagement),
//            NormalizedName = Enum.GetName(RoleEnum.UserManagement).Normalize(),
//            DisplayName = RoleEnum.UserManagement.GetDescription()
//        });
//        builder.HasData(new Role
//        {
//            Id = RoleEnum.Province.GetId(),
//            Name = Enum.GetName(RoleEnum.Province),
//            NormalizedName = Enum.GetName(RoleEnum.Province).Normalize(),
//            DisplayName = RoleEnum.Province.GetDescription()
//        });
//    }
//}