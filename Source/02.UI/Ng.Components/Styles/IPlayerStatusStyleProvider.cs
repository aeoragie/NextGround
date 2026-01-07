using Ng.Domain.Enums;

namespace Ng.Components.Styles
{
    public interface IPlayerStatusStyleProvider
    {
        // 클래스 이름을 반환 (Tailwind, Bootstrap 등)
        string GetClass(PlayerStatus status);

        // 인라인 스타일을 반환 (필요한 경우)
        string GetInlineStyle(PlayerStatus status);
    }
}
