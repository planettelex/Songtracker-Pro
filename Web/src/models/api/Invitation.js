import Model from './Model';

export default class Invitation extends Model {
    resource() {
        return 'invitations';
      }

      primaryKey() {
        return 'uuid';
      }
}