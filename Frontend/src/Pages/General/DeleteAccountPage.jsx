import { DeleteAccountForm } from "../../Components/Forms/DeleteAccountForm";
import deleteAccount from "../../assets/Generals/delete-account.jpg";

const DeleteAccountPage = () => {
  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 gap-4 h-screen w-full">
      <DeleteAccountForm />
      <div className="hidden sm:block">
        <img
          className="w-full h-full object-cover"
          src={deleteAccount}
          alt="regiter cam image"
        />
      </div>
    </div>
  );
};

export default DeleteAccountPage;
