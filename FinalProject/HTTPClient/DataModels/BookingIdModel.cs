﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient.DataModels
{
    class BookingIdModel
    {
        [JsonProperty("bookingid")]
        public int BookingId { get; set;}

        [JsonProperty("booking")]
        public BookingDetailsModel Booking { get; set;}
    }
}
