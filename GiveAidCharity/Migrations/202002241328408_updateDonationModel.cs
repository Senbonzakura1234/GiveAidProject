namespace GiveAidCharity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDonationModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Donations", "vnp_BankCode", c => c.String());
            AddColumn("dbo.Donations", "vnp_BankTranNo", c => c.String());
            AddColumn("dbo.Donations", "vnp_CardType", c => c.String());
            AddColumn("dbo.Donations", "vnp_OrderInfo", c => c.String());
            AddColumn("dbo.Donations", "vnp_PayDate", c => c.String());
            AddColumn("dbo.Donations", "vnp_TmnCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Donations", "vnp_TmnCode");
            DropColumn("dbo.Donations", "vnp_PayDate");
            DropColumn("dbo.Donations", "vnp_OrderInfo");
            DropColumn("dbo.Donations", "vnp_CardType");
            DropColumn("dbo.Donations", "vnp_BankTranNo");
            DropColumn("dbo.Donations", "vnp_BankCode");
        }
    }
}
