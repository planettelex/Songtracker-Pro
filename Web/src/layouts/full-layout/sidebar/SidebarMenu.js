import userType from "../../../usertype";
function getSidebarMenu(type)
{
    switch(type) {
        case userType.SystemAdministrator:
          // code block
          break;
        case userType.LabelAdministrator:
          // code block
          break;
        case userType.PublisherAdministrator:
          // code block
          break;
        case userType.SystemUser:
            return [
                {
                    icon: 'mdi-file',
                    title: 'Start Page',
                    to: '/starterpage',
                },
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