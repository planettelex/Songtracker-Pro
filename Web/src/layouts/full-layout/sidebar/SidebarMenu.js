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
                    titleKey: 'PublishingCompany',
                    to: '/publishing-companies',
                },
                {
                    icon: 'mdi-label-multiple',
                    titleKey: 'RecordLabel',
                    to: '/record-labels',
                },
                {
                    icon: 'mdi-cloud',
                    titleKey: 'Platform',
                    to: '/platforms',
                },
                {
                    icon: 'mdi-music',
                    titleKey: 'Artist',
                    to: '/artists',
                },
                {
                    icon: 'mdi-account-multiple',
                    titleKey: 'User',
                    to: '/users',
                },
                {
                    icon: 'mdi-email',
                    titleKey: 'Invitation',
                    to: '/invitations'
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
                    titleKey: 'Document',
                    to: '/label-documents',
                },
                {
                    icon: 'mdi-microphone',
                    titleKey: 'Recording',
                    to: '/label-recordings',
                },
                {
                    icon: 'mdi-album',
                    titleKey: 'Release',
                    to: '/label-releases',
                },
                {
                    icon: 'mdi-music',
                    titleKey: 'Artist',
                    to: '/label-artists',
                },
                {
                    icon: 'mdi-account-multiple',
                    titleKey: 'User',
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
                    titleKey: 'Document',
                    to: '/publisher-documents',
                },
                {
                    icon: 'mdi-music-box-multiple',
                    titleKey: 'Composition',
                    to: '/publisher-compositions',
                },
                {
                    icon: 'mdi-account-multiple',
                    titleKey: 'User',
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
                    titleKey: 'Document',
                    to: '/my-documents',
                },
                {
                    icon: 'mdi-music-box-multiple',
                    titleKey: 'Composition',
                    to: '/my-compositions',
                },
                {
                    icon: 'mdi-microphone',
                    titleKey: 'Recording',
                    to: '/my-recordings',
                },
                {
                    icon: 'mdi-album',
                    titleKey: 'Release',
                    to: '/my-releases',
                },
            ];
        default:
          // no menu
      }
}
export { getSidebarMenu as default };