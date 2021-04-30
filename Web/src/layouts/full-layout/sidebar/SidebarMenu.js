import UserType from "../../../user-type";
function getSidebarMenu(type)
{
    switch(type) {
        case UserType.SystemAdministrator:
            return [
                {
                    icon: 'mdi-file',
                    title: 'System Information',
                    to: '/system-information',
                },
            ];
        case UserType.LabelAdministrator:
            return [
                {
                    icon: 'mdi-file',
                    title: 'Earnings',
                    to: '/label-earnings',
                },
            ];
        case UserType.PublisherAdministrator:
            return [
                {
                    icon: 'mdi-file',
                    title: 'Earnings',
                    to: '/publisher-earnings',
                },
            ];
        case UserType.SystemUser:
            return [
                {
                    icon: 'mdi-file',
                    title: 'Earnings',
                    to: '/my-earnings',
                },
            ];
        default:
          // no menu
      }
}
export { getSidebarMenu as default };