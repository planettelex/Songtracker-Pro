import UserType from "../../../enums/UserType";
function getSidebarMenu(userType)
{
    switch(userType) {
        case UserType.SystemAdministrator:
            return [
                {
                    icon: 'mdi-information',
                    titleKey: 'SystemInformation',
                    to: '/system-information',
                },
                {
                    icon: 'mdi-book-open-variant',
                    titleKey: 'PublishingCompanies',
                    to: '/publishing-companies',
                },
                {
                    icon: 'mdi-label-multiple',
                    titleKey: 'RecordLabels',
                    to: '/record-labels',
                },
                {
                    icon: 'mdi-cloud',
                    titleKey: 'Platforms',
                    to: '/platforms',
                },
                {
                    icon: 'mdi-music',
                    titleKey: 'Artists',
                    to: '/artists',
                },
                {
                    icon: 'mdi-account-multiple',
                    titleKey: 'Users',
                    to: '/users',
                }
            ];
        case UserType.LabelAdministrator:
            return [
                {
                    icon: 'mdi-bank',
                    titleKey: 'Earnings',
                    to: '/label-earnings',
                },
                {
                    icon: 'mdi-folder',
                    titleKey: 'Documents',
                    to: '/label-documents',
                },
                {
                    icon: 'mdi-microphone',
                    titleKey: 'Recordings',
                    to: '/label-recordings',
                },
                {
                    icon: 'mdi-album',
                    titleKey: 'Releases',
                    to: '/label-releases',
                },
                {
                    icon: 'mdi-music',
                    titleKey: 'Artists',
                    to: '/label-artists',
                },
                {
                    icon: 'mdi-account-multiple',
                    titleKey: 'Users',
                    to: '/label-users',
                },
                {
                    icon: 'mdi-information-variant',
                    titleKey: 'Information',
                    to: '/label-information',
                }
            ];
        case UserType.PublisherAdministrator:
            return [
                {
                    icon: 'mdi-bank',
                    titleKey: 'Earnings',
                    to: '/publisher-earnings',
                },
                {
                    icon: 'mdi-folder',
                    titleKey: 'Documents',
                    to: '/publisher-documents',
                },
                {
                    icon: 'mdi-music-box-multiple',
                    titleKey: 'Compositions',
                    to: '/publisher-compositions',
                },
                {
                    icon: 'mdi-account-multiple',
                    titleKey: 'Users',
                    to: '/publisher-users',
                },
                {
                    icon: 'mdi-information-variant',
                    titleKey: 'Information',
                    to: '/publisher-information',
                },
            ];
        case UserType.SystemUser:
            return [
                {
                    icon: 'mdi-bank',
                    titleKey: 'Earnings',
                    to: '/my-earnings',
                },
                {
                    icon: 'mdi-folder',
                    titleKey: 'Documents',
                    to: '/my-documents',
                },
                {
                    icon: 'mdi-music-box-multiple',
                    titleKey: 'Compositions',
                    to: '/my-compositions',
                },
                {
                    icon: 'mdi-microphone',
                    titleKey: 'Recordings',
                    to: '/my-recordings',
                },
                {
                    icon: 'mdi-album',
                    titleKey: 'Releases',
                    to: '/my-releases',
                },
            ];
        default:
          // no menu
      }
}
export { getSidebarMenu as default };