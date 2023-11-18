
namespace WebApiProxy.Data.Dto
{
    public class CreateLogApplicationIn
    {
        public int aud_IdRequest { get; set; }
        public string aud_idAplication { get; set; }
        public string aud_class { get; set; }
        public string aud_method { get; set; }
        public string aud_status { get; set; }
        public Object aud_data_before { get; set; }
        public Object aud_data_after { get; set; }
        public string aud_message { get; set; }
        public string aud_exception { get; set; }
        public string aud_ip { get; set; }
    }
}
