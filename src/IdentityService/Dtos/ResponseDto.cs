namespace IdentityService.Dtos;

public class ResponseDto<T>
{
    public ResponseDto(T data)
    {
        this.Data = data;    
    }

    public T Data {get; set;}
}