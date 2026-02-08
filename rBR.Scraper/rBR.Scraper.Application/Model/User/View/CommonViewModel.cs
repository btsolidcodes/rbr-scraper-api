using rBR.BaseLibraries.Application.Model.View;
using rBR.BaseLibraries.Domain.Enumerator;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace rBR.Scraper.Application.Model.User.View
{
    /// <summary>
    /// The class that holds the visualization properties of the applications' objects common properties.
    /// </summary>
    public class CommonViewModel : BaseCommonViewModel
    {
        /// <summary>
        /// The <see cref="CommonViewModel"/>'s default constructor.
        /// </summary>
        /// <param name="id">The data model objects' id.</param>
        /// <param name="status">The data model objects' status.</param>
        /// <param name="created">The data model objects' creation date and time.</param>
        /// <param name="modified">The data model objects' last modification date and time.</param>
        public CommonViewModel(Guid id, int status, DateTimeOffset created, DateTimeOffset? modified) : base(id, status, created, modified)
        {
            Status = DescribedStatus(status);
            StatusId = status;
        }

        /// <summary>
        /// The static method for retrieving the statuses' descriptions.
        /// </summary>
        /// <param name="statusId">The id of the status.</param>
        /// <returns>A <see cref="string"/> object for the description of the status.</returns>
        public new static string DescribedStatus(int statusId)
        {
            var status = Enum.GetValues(typeof(BaseStatus.EnumStatus)).GetValue(statusId - 1);
            string describedStatuses = ((DescriptionAttribute[])status.GetType().GetTypeInfo().GetField(status.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false))[0].Description;

            CultureInfo culturePortuguese = new("pt-BR");
            if (CultureInfo.CurrentCulture.Name == culturePortuguese.Name)
                return describedStatuses.Split(',')[1];
            else
                return describedStatuses.Split(',')[0];
        }
    }
}
