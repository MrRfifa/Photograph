import { ErrorMessage, Field, Form, Formik } from "formik";
import * as Yup from "yup";
import PropTypes from "prop-types";
import { useState } from "react";
import UserService from "../../Services/User/UserService";
import toast from "react-hot-toast";

const ChangeNamesForm = ({ initialLastname, initialFirstname }) => {
  const [loading, setLoading] = useState(false);

  async function onFinish(values) {
    try {
      setLoading(true);
      const response = await UserService.changeNames(
        values.password,
        values.firstname,
        values.lastname
      );
      console.log(response);
      if (response.success) {
        toast.success(response.message, {
          duration: 4500,
          position: "top-right",
          icon: "🔥",
          className: "bg-green-500 text-white",
        });
        window.location.reload("/");
      } else {
        toast.error("Names change failed: " + response.message, {
          duration: 2500,
          position: "top-right",
          icon: "💀",
          className: "bg-yellow-500 text-white",
        });
      }
    } catch (error) {
      toast.error(
        "An error occurred during name change. Please try again later.",
        {
          duration: 2000,
          position: "top-right",
          icon: "🤌🏻",
          className: "bg-red-500 text-white",
        }
      );
    } finally {
      setLoading(false);
    }
  }

  return (
    <div>
      <h2 className="text-2xl pl-[25%] font-bold text-white mb-4">
        Change names
      </h2>
      <Formik
        initialValues={{
          password: "",
          firstname: initialFirstname,
          lastname: initialLastname,
        }}
        validationSchema={Yup.object({
          password: Yup.string().required("Password is required"),
          firstname: Yup.string().required("Firstname is required"),
          lastname: Yup.string().required("Lastname is required"),
        })}
        onSubmit={onFinish}
      >
        <Form className="px-7 grid place-items-center">
          <div className="grid gap-6" id="form">
            <div className="grid gap-6 w-full">
              <label className="text-white" htmlFor="password">
                Password
              </label>
              <Field
                type="password"
                id="password"
                name="password"
                className="p-3 w-full placeholder-black text-black rounded-2xl focus:border-solid focus.border-2 "
                required
              />
              <ErrorMessage
                name="password"
                component="div"
                className="text-red-500"
              />
            </div>
            <div className="grid gap-6 w-full">
              <label className="text-white" htmlFor="firstname">
                Firstname
              </label>
              <Field
                type="text"
                id="firstname"
                name="firstname"
                className="p-3 w-full placeholder-black text-black rounded-2xl focus:border-solid focus.border-2 "
                required
              />
              <ErrorMessage
                name="firstname"
                component="div"
                className="text-red-500"
              />
            </div>
            <div className="grid gap-6 w-full">
              <label className="text-white" htmlFor="lastname">
                Lastname
              </label>
              <Field
                type="text"
                id="lastname"
                name="lastname"
                className="p-3 w-full placeholder-black text-black rounded-2xl focus:border-solid focus.border-2 "
                required
              />
              <ErrorMessage
                name="lastname"
                component="div"
                className="text-red-500"
              />
            </div>
            <button
              className="outline-none shadow-2xl w-full p-3 bg-blue-400 hover:border-blue-500 hover:border-solid hover:border-2 hover:text-blue-500 font-bold"
              type="submit"
            >
              {loading ? "Updating..." : "Update"}
            </button>
          </div>
        </Form>
      </Formik>
    </div>
  );
};

export default ChangeNamesForm;

ChangeNamesForm.propTypes = {
  initialLastname: PropTypes.string.isRequired,
  initialFirstname: PropTypes.string.isRequired,
};
