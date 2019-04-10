using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Customer")]
    public class ImportCustomerDto
    {
        //      <Customers>
        //<Customer>
        //  <FirstName>Randi</FirstName>
        //  <LastName>Ferraraccio</LastName>
        //  <Age>20</Age>
        //  <Balance>59.44</Balance>
        //  <Tickets>
        //    <Ticket>
        //      <ProjectionId>1</ProjectionId>
        //      <Price>7</Price>
        //    </Ticket>
        //    <Ticket>
        //      <ProjectionId>1</ProjectionId>
        //      <Price>15</Price>
        //    </Ticket>
        //    <Ticket>
        //      <ProjectionId>1</ProjectionId>
        //      <Price>12.13</Price>
        //    </Ticket>
        //</Customer>
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        [XmlElement("LastName")]
        public string LastName { get; set; }

        [Range(12, 110)]
        [XmlElement("Age")]
        [Required]
        public int Age { get; set; }

        [XmlElement("Balance")]
        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Balance { get; set; }

        [XmlArray("Tickets")]
        public TicketDto[] Tickets { get; set; }
    }

    [XmlType("Ticket")]
    public class TicketDto
    {
        //      <ProjectionId>1</ProjectionId>
        //      <Price>12.13</Price>
        [XmlElement("ProjectionId")]
        [Required]
        public int ProjectionId { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        [XmlElement("Price")]
        [Required]
        public decimal Price { get; set; }
    }
}
