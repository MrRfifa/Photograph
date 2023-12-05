import { Link, useNavigate } from "react-router-dom";
import { Formik, Field, Form, ErrorMessage } from "formik";
import * as Yup from "yup";
import { FaArrowCircleLeft } from "react-icons/fa";
import AuthService from "../../Services/Auth/AuthService";
import toast, { Toaster } from "react-hot-toast";
import { useState } from "react";

const RegisterForm = () => {
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  async function onFinish(values) {
    try {
      setLoading(true);
      const response = await AuthService.register(
        values.firstName,
        values.lastName,
        values.gender,
        values.age,
        values.email,
        values.password,
        values.confirmPassword
      );
      if (response.success) {
        toast.success(
          "Registred successfully! \n\n A verification mail is send to the provided mail.",
          {
            duration: 4500,
            position: "top-right",
            icon: "🔥",
            className: "bg-green-500 text-white",
          }
        );
        setTimeout(() => {
          navigate("/login");
        }, 5000);
      } else {
        toast.error("Registration failed. Please check your credentials.", {
          duration: 2000,
          position: "top-right",
          icon: "💀",
          className: "bg-yellow-500 text-white",
        });
      }
    } catch (error) {
      toast.error("An error occurred during login. Please try again later.", {
        duration: 2000,
        position: "top-right",
        icon: "🤌🏻",
        className: "bg-red-500 text-white",
      });
    } finally {
      setLoading(false);
    }
  }

  return (
    <>
      <Toaster />
      <div className="flex flex-col items-center justify-center h-screen dark">
        <div className="w-full max-w-md bg-gray-800 rounded-lg shadow-md p-6">
          <div className="flex flex-row justify-start">
            <Link to="/">
              <FaArrowCircleLeft size={40} color="white" />
            </Link>
            <h2 className="text-2xl pl-[25%] font-bold text-gray-200 mb-4">
              Sign Up
            </h2>
          </div>
          <Formik
            initialValues={{
              firstName: "",
              lastName: "",
              email: "",
              password: "",
              confirmPassword: "",
              gender: "male",
              age: new Date(),
            }}
            validationSchema={Yup.object({
              firstName: Yup.string().required("First Name is required"),
              lastName: Yup.string().required("Last Name is required"),
              email: Yup.string()
                .email("Invalid email address")
                .required("Email is required"),
              password: Yup.string()
                .min(6, "Password must be at least 6 characters")
                .required("Password is required")
                .matches(
                  /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/,
                  "Password must contain at least one uppercase letter, one lowercase letter, and one number"
                ),
              confirmPassword: Yup.string()
                .oneOf([Yup.ref("password"), null], "Passwords must match")
                .required("Confirm Password is required"),
              gender: Yup.string().required("Gender is required"),
              age: Yup.date().required("Age is required"),
            })}
            onSubmit={onFinish}
          >
            <Form className="flex flex-col">
              <div className="flex space-x-4 mb-4">
                <div>
                  <Field
                    type="text"
                    name="firstName"
                    placeholder="First Name"
                    className="bg-gray-700 text-gray-200 border-0 rounded-md p-2 w-[100%] focus:bg-gray-600 focus:outline-none focus:ring-1 focus:ring-blue-500 transition ease-in-out duration-150"
                  />
                  <ErrorMessage
                    name="firstName"
                    component="div"
                    className="text-red-500"
                  />
                </div>
                <div>
                  <Field
                    type="text"
                    name="lastName"
                    placeholder="Last Name"
                    className="bg-gray-700 text-gray-200 border-0 rounded-md p-2 w-[100%] focus:bg-gray-600 focus:outline-none focus:ring-1 focus:ring-blue-500 transition ease-in-out duration-150"
                  />
                  <ErrorMessage
                    name="lastName"
                    component="div"
                    className="text-red-500"
                  />
                </div>
              </div>

              <div>
                <Field
                  type="email"
                  name="email"
                  placeholder="Email"
                  className="bg-gray-700 text-gray-200 border-0 w-[100%] rounded-md p-2 mb-4 focus:bg-gray-600 focus:outline-none focus:ring-1 focus:ring-blue-500 transition ease-in-out duration-150"
                />
                <ErrorMessage
                  name="email"
                  component="div"
                  className="text-red-500"
                />
              </div>

              <div>
                <Field
                  type="password"
                  name="password"
                  placeholder="Password"
                  className="bg-gray-700 text-gray-200 border-0 rounded-md w-[100%] p-2 mb-4 focus:bg-gray-600 focus:outline-none focus:ring-1 focus:ring-blue-500 transition ease-in-out duration-150"
                />
                <ErrorMessage
                  name="password"
                  component="div"
                  className="text-red-500"
                />
              </div>

              <div>
                <Field
                  type="password"
                  name="confirmPassword"
                  placeholder="Confirm Password"
                  className="bg-gray-700 text-gray-200 border-0 w-[100%] rounded-md p-2 mb-4 focus:bg-gray-600 focus:outline-none focus:ring-1 focus:ring-blue-500 transition ease-in-out duration-150"
                />
                <ErrorMessage
                  name="confirmPassword"
                  component="div"
                  className="text-red-500"
                />
              </div>

              <div>
                <label
                  className="text-sm mb-2 text-gray-200  cursor-pointer"
                  htmlFor="gender"
                >
                  Gender
                </label>
                <Field
                  as="select"
                  name="gender"
                  className="bg-gray-700 text-gray-200 w-[100%] border-0 rounded-md p-2 mb-4 focus:bg-gray-600 focus:outline-none focus:ring-1 focus:ring-blue-500 transition ease-in-out duration-150"
                >
                  <option value="male">Male</option>
                  <option value="female">Female</option>
                </Field>
                <ErrorMessage
                  name="gender"
                  component="div"
                  className="text-red-500"
                />
              </div>

              <div>
                <label
                  className="text-sm mb-2 text-gray-200 cursor-pointer"
                  htmlFor="age"
                >
                  Date Of Birth
                </label>
                <Field
                  type="date"
                  name="age"
                  className="bg-gray-700 w-[100%] text-gray-200 border-0 rounded-md p-2"
                />
                <ErrorMessage
                  name="age"
                  component="div"
                  className="text-red-500"
                />
              </div>

              <p className="text-white mt-4">
                Already have an account?{" "}
                <Link
                  to="/login"
                  className="text-sm text-blue-500 -200 hover:underline mt-4"
                >
                  Login
                </Link>
              </p>
              <button
                className="bg-gradient-to-r from-[#E0B1CB] to-[#240046] text-white font-bold py-2 px-4 rounded-md mt-4 hover:bg-indigo-600 hover:to-[#7B2CBF] transition ease-in-out duration-150"
                type="submit"
              >
                {loading ? "Signing up..." : "Sign Up"}
              </button>
            </Form>
          </Formik>
        </div>
      </div>
    </>
  );
};

export default RegisterForm;
