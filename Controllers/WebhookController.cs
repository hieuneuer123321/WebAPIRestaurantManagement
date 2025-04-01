using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Supabase.Interfaces;

namespace WebAPIRestaurantManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        [HttpPost("email-confirmed")]
        public async Task<IActionResult> HandleEmailConfirmed([FromBody] SupabaseEvent eventData)
        {
            // Kiểm tra dữ liệu event nhận được từ Supabase
            if (eventData.Type == "email_confirmed" && eventData.User != null)
            {
                // Lưu email vào bảng của bạn
                string email = eventData.User.Email;

                // Giả sử bạn có một dịch vụ để lưu email vào cơ sở dữ liệu
                //await SaveEmailToDatabase(email);

                return Ok(new { message = "Email confirmed and saved successfully." });
            }

            return BadRequest("Invalid event data");
        }

        private async Task SaveEmailToDatabase(string email)
        {
            //// Ví dụ: gọi dịch vụ để lưu email vào bảng khác trong Supabase
            //// Thực hiện lưu email vào bảng của bạn tại Supabase
            //var response = await _supabaseClient
            //    .From<EmailTable>()  // Thay EmailTable bằng tên bảng của bạn
            //    .Insert(new { Email = email });

            //// Kiểm tra và xử lý kết quả từ Supabase
            //if (response.Error != null)
            //{
            //    // Xử lý lỗi khi lưu vào cơ sở dữ liệu
            //    throw new Exception($"Error saving email: {response.Error.Message}");
            //}
        }
        public class SupabaseEvent
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("user")]
            public SupabaseUser User { get; set; }
        }

        public class SupabaseUser
        {
            [JsonProperty("email")]
            public string Email { get; set; }
        }
    }
}
