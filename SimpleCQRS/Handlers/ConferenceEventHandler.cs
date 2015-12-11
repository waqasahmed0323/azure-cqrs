﻿using SimpleCQRS.Events;
using SimpleCQRS.Infrastructure;
using SimpleCQRS.Infrastructure.Query;

namespace SimpleCQRS.Handlers
{
    public class ConferenceEventHandler : 
        IHandles<AttendeeRegistered>,
        IHandles<AttendeeChangeEmailConfirmed>,
        IHandles<AttendeeEmailChanged>,
        IHandles<AttendeeUnregistered>
    {

        private readonly IDataAccess<AttendeeEntity> _dataAccess;

        public ConferenceEventHandler(IDataAccess<AttendeeEntity> dataAccess)
        {
            _dataAccess = dataAccess;
        }

        void IHandles<AttendeeRegistered>.Handle(AttendeeRegistered message)
        {
            var attendee = _dataAccess.GetById(message.Id);

            if (attendee == null)
            {
                attendee = new AttendeeEntity(message.Id, message.Email);
                _dataAccess.Insert(attendee);
            }
        }

        void IHandles<AttendeeUnregistered>.Handle(AttendeeUnregistered message)
        {
            var attendee = _dataAccess.GetById(message.Id);

            if (attendee != null)
            {
                attendee.IsActiveRegistration = false;
                attendee.ReasonForUnregistration = message.Reason;
                _dataAccess.Update(attendee);
            }
        }

        void IHandles<AttendeeChangeEmailConfirmed>.Handle(AttendeeChangeEmailConfirmed message)
        {
            var attendee = _dataAccess.GetById(message.Id);

            if (attendee != null)
            {
                attendee.Email = message.Email;
                _dataAccess.Update(attendee);
            }
        }

        void IHandles<AttendeeEmailChanged>.Handle(AttendeeEmailChanged message)
        {
            //nothing to do yet
        }
    }
}