import userType from "../../../usertype";
function getSidebarMenu(type)
{
    switch(type) {
        case userType.SystemAdministrator:
            return [
                {
                    icon: 'mdi-file',
                    title: 'System Information',
                    to: '/system-information',
                },
            ];
        case userType.LabelAdministrator:
            return [
                {
                    icon: 'mdi-file',
                    title: 'Earnings',
                    to: '/label-earnings',
                },
            ];
        case userType.PublisherAdministrator:
            return [
                {
                    icon: 'mdi-file',
                    title: 'Earnings',
                    to: '/publisher-earnings',
                },
            ];
        case userType.SystemUser:
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