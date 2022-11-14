using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

// В проектах SDK, таких как этот, некоторые атрибуты сборки, которые ранее определялись
// в этом файле, теперь автоматически добавляются во время сборки и заполняются значениями,
// заданными в свойствах проекта. Подробные сведения о том, какие атрибуты включены
// и как настроить этот процесс, см. на странице: https://aka.ms/assembly-info-properties.


// При установке значения false для параметра ComVisible типы в этой сборке становятся
// невидимыми для компонентов COM. Если вам необходимо получить доступ к типу в этой
// сборке из модели COM, установите значение true для атрибута ComVisible этого типа.

[assembly: ComVisible(false)]

// Следующий GUID служит для идентификации библиотеки типов typelib, если этот проект
// будет видимым для COM.

[assembly: Guid("a5282f42-d0ce-411d-9774-b3c05d84a742")]

[assembly: InternalsVisibleTo("TicketManagement.IntegrationTests")]
[assembly: InternalsVisibleTo("TicketManagement.UnitTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]