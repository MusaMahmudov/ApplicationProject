using AbilloLLCApplication.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Database.Configurations
{
    public class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.Property(d=>d.Length).IsRequired();
            builder.Property(d => d.Width).IsRequired(); 
            builder.Property(d => d.Height).IsRequired();
            builder.Property(d=>d.PhoneNumber).IsRequired().HasMaxLength(128);
         

            builder.HasCheckConstraint("TelegramUserNameLimit", "Len(TelegramUserName) > 2");
            builder.HasCheckConstraint("TelegramUserIdLimit", "Len(TelegramUserId) > 6");

            builder.HasCheckConstraint("LengthPositive","Length > 0");
            builder.HasCheckConstraint("WidthPositive", "Width > 0");
            builder.HasCheckConstraint("HeightPositive", "Height > 0");


        }
    }
}
